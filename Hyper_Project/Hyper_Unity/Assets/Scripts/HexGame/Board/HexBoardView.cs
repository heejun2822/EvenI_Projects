using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>Renders the tilemap, formula labels, and move glows for a board state.</summary>
public sealed class HexBoardView
{
    private readonly Tilemap tilemap;
    private readonly TileBase normalTile;
    private readonly TextMeshPro formulaLabelPrefab;
    private readonly SpriteRenderer glowPrefab;
    private readonly Transform labelRoot;
    private readonly Transform glowRoot;
    private readonly Color defaultTileColor;
    private readonly Color goalTileColor;
    private readonly Color healthTileColor;
    private readonly Color unavailableTileColor;
    private readonly Dictionary<Vector3Int, TextMeshPro> labels = new();
    private readonly List<SpriteRenderer> glows = new();

    public HexBoardView(Tilemap tilemap, TileBase normalTile, TextMeshPro formulaLabelPrefab, SpriteRenderer glowPrefab,
        Transform labelRoot, Transform glowRoot, Color defaultTileColor, Color goalTileColor, Color healthTileColor, Color unavailableTileColor)
    {
        this.tilemap = tilemap;
        this.normalTile = normalTile;
        this.formulaLabelPrefab = formulaLabelPrefab;
        this.glowPrefab = glowPrefab;
        this.labelRoot = labelRoot;
        this.glowRoot = glowRoot;
        this.defaultTileColor = defaultTileColor;
        this.goalTileColor = goalTileColor;
        this.healthTileColor = healthTileColor;
        this.unavailableTileColor = unavailableTileColor;
    }

    public void Build(HexBoardState board)
    {
        Clear();
        foreach (Vector3Int cell in board.Cells)
        {
            SetTile(cell, board.IsGoal(cell) ? goalTileColor : defaultTileColor);
            if (board.IsGoal(cell))
            {
                CreateLabel(cell, $"GOAL\n{board.Stage.GoalScore}");
            }
            else if (board.TryGetFormula(cell, out FormulaTileData formula))
            {
                CreateLabel(cell, formula.ToString());
            }
            else if (board.TryGetHealth(cell, out int healthAmount))
            {
                SetTile(cell, healthTileColor);
                CreateLabel(cell, $"HEAL\n+{healthAmount}");
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
                SetTile(cell, unavailableTileColor);
            }
            else if (!board.IsVisited(cell))
            {
                SetTile(cell, board.IsGoal(cell) ? goalTileColor : board.IsHealthCell(cell) ? healthTileColor : defaultTileColor);
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

    private void SetTile(Vector3Int cell, Color color)
    {
        tilemap.SetTile(cell, normalTile);
        tilemap.SetTileFlags(cell, TileFlags.None);
        tilemap.SetColor(cell, color);
    }

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
