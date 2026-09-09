using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>Renders the tilemap, formula labels, and move glows for a board state.</summary>
public sealed class HexBoardView : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase defaultTile;
    [SerializeField] private TileBase unavailableTile;
    [SerializeField] private TileBase itemTile;
    [SerializeField] private TileBase goalTile;
    [SerializeField] private TextMeshPro formulaLabelPrefab;
    [SerializeField] private SpriteRenderer glowPrefab;
    [SerializeField] private Transform labelRoot;
    [SerializeField] private Transform glowRoot;
    private readonly Dictionary<Vector3Int, TextMeshPro> labels = new();
    private readonly List<SpriteRenderer> glows = new();

    public Tilemap Tilemap => tilemap;
    public bool IsConfigured => tilemap != null && defaultTile != null && unavailableTile != null && itemTile != null &&
        goalTile != null && formulaLabelPrefab != null && glowPrefab != null && labelRoot != null && glowRoot != null;

    public void Build(HexBoardState board)
    {
        Clear();
        foreach (Vector3Int cell in board.Cells)
        {
            if (board.IsGoal(cell))
            {
                SetTile(cell, goalTile);
                CreateLabel(cell, $"{board.Stage.GoalScore}");
            }
            else if (board.TryGetFormula(cell, out FormulaTileData formula))
            {
                SetTile(cell, itemTile);
                CreateLabel(cell, formula.ToString());
            }
            else if (board.TryGetHealth(cell, out int healthAmount))
            {
                SetTile(cell, itemTile);
                CreateLabel(cell, $"{healthAmount}");
            }
            else
            {
                SetTile(cell, defaultTile);
            }
        }
    }

    public void Refresh(HexBoardState board)
    {
        ClearGlows();
        foreach (Vector3Int cell in board.Cells)
        {
            if (board.IsUnavailable(cell))
            {
                SetTile(cell, unavailableTile);
            }
            else if (!board.IsVisited(cell))
            {
                SetTile(cell, GetAvailableTile(board, cell));
            }
        }

        foreach (Vector3Int neighbour in HexPathfinder.GetNeighbours(board.CurrentCell))
        {
            if (board.CanMove(neighbour))
            {
                SpriteRenderer glow = Object.Instantiate(glowPrefab, tilemap.GetCellCenterWorld(neighbour), Quaternion.identity, glowRoot);
                glows.Add(glow);
            }
        }
    }

    public void RemoveTile(Vector3Int cell)
    {
        tilemap.SetTile(cell, null);
    }

    public void RemoveLabel(Vector3Int cell)
    {
        if (labels.Remove(cell, out TextMeshPro label))
        {
            Object.Destroy(label.gameObject);
        }
    }

    public Bounds CalculateBounds(HexBoardState board)
    {
        Vector3 cellSize = tilemap.cellSize;
        Bounds bounds = new(tilemap.GetCellCenterWorld(board.CurrentCell), cellSize);
        foreach (Vector3Int cell in board.Cells)
        {
            Vector3 center = tilemap.GetCellCenterWorld(cell);
            bounds.Encapsulate(center - cellSize * .5f);
            bounds.Encapsulate(center + cellSize * .5f);
        }
        return bounds;
    }

    public void Clear()
    {
        tilemap.ClearAllTiles();
        foreach (TextMeshPro label in labels.Values)
        {
            Object.Destroy(label.gameObject);
        }
        labels.Clear();
        ClearGlows();
    }

    private TileBase GetAvailableTile(HexBoardState board, Vector3Int cell)
    {
        if (board.IsGoal(cell))
        {
            return goalTile;
        }

        return board.IsHealthCell(cell) || board.TryGetFormula(cell, out _) ? itemTile : defaultTile;
    }

    private void SetTile(Vector3Int cell, TileBase tile) => tilemap.SetTile(cell, tile);

    private void CreateLabel(Vector3Int cell, string value)
    {
        TextMeshPro label = Object.Instantiate(formulaLabelPrefab, tilemap.GetCellCenterWorld(cell), Quaternion.identity, labelRoot);
        label.text = value;
        labels.Add(cell, label);
    }

    private void ClearGlows()
    {
        foreach (SpriteRenderer glow in glows)
        {
            Object.Destroy(glow.gameObject);
        }
        glows.Clear();
    }
}
