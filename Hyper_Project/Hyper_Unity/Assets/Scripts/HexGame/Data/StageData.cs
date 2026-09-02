using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hex Game/Stage Data", fileName = "StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] private List<FormulaTileData> formulaTiles = new();
    [SerializeField] private List<HealthTileData> healthTiles = new();
    [SerializeField] private Vector3Int startCell;
    [SerializeField] private Vector3Int goalCell;
    [SerializeField] private int goalScore;

    public IReadOnlyList<FormulaTileData> FormulaTiles => formulaTiles;
    public IReadOnlyList<HealthTileData> HealthTiles => healthTiles;
    public Vector3Int StartCell => startCell;
    public Vector3Int GoalCell => goalCell;
    public int GoalScore => goalScore;

    public bool Validate(out string error)
    {
        HashSet<Vector3Int> cells = new();
        foreach (FormulaTileData tile in formulaTiles)
        {
            if (!cells.Add(tile.cell))
            {
                error = "More than one content tile uses the same cell.";
                return false;
            }

            if (tile.operation == FormulaOperator.Divide && tile.operand == 0)
            {
                error = "A formula tile cannot divide by zero.";
                return false;
            }
        }

        foreach (HealthTileData tile in healthTiles)
        {
            if (!cells.Add(tile.cell))
            {
                error = "More than one content tile uses the same cell.";
                return false;
            }

            if (tile.healthAmount <= 0)
            {
                error = "A health tile must restore at least one health.";
                return false;
            }
        }

        if (cells.Contains(startCell))
        {
            error = "The start cell must not contain a content tile.";
            return false;
        }

        if (goalCell == startCell || cells.Contains(goalCell))
        {
            error = "The goal cell must not overlap the start or a content tile.";
            return false;
        }

        HashSet<Vector3Int> board = new(cells) { startCell, goalCell };
        if (!HexPathfinder.HasPath(startCell, goalCell, board, new HashSet<Vector3Int>()))
        {
            error = "There is no path from the start cell to the goal.";
            return false;
        }

        error = string.Empty;
        return true;
    }
}
