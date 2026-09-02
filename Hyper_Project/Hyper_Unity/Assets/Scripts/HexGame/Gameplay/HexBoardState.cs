using System.Collections.Generic;
using UnityEngine;

/// <summary>Owns the rule-only state of the current stage board.</summary>
public sealed class HexBoardState
{
    private readonly Dictionary<Vector3Int, FormulaTileData> formulas = new();
    private readonly Dictionary<Vector3Int, int> healthTiles = new();
    private readonly HashSet<Vector3Int> cells = new();
    private readonly HashSet<Vector3Int> visited = new();
    private readonly HashSet<Vector3Int> unavailable = new();

    public StageData Stage { get; }
    public Vector3Int CurrentCell { get; private set; }
    public IEnumerable<Vector3Int> Cells => cells;

    public HexBoardState(StageData stage)
    {
        Stage = stage;
        foreach (FormulaTileData formula in stage.FormulaTiles)
        {
            formulas.Add(formula.cell, formula);
            cells.Add(formula.cell);
        }
        foreach (HealthTileData healthTile in stage.HealthTiles)
        {
            healthTiles.Add(healthTile.cell, healthTile.healthAmount);
            cells.Add(healthTile.cell);
        }

        cells.Add(stage.StartCell);
        cells.Add(stage.GoalCell);
        CurrentCell = stage.StartCell;
        visited.Add(CurrentCell);
    }

    public bool IsGoal(Vector3Int cell) => cell == Stage.GoalCell;
    public bool IsVisited(Vector3Int cell) => visited.Contains(cell);
    public bool IsUnavailable(Vector3Int cell) => unavailable.Contains(cell);
    public bool TryGetFormula(Vector3Int cell, out FormulaTileData formula) => formulas.TryGetValue(cell, out formula);
    public bool TryGetHealth(Vector3Int cell, out int healthAmount) => healthTiles.TryGetValue(cell, out healthAmount);
    public bool IsHealthCell(Vector3Int cell) => healthTiles.ContainsKey(cell);

    public bool TryMove(Vector3Int target, out Vector3Int previousCell)
    {
        previousCell = CurrentCell;
        if (!CanMove(target))
        {
            return false;
        }

        CurrentCell = target;
        visited.Add(CurrentCell);
        return true;
    }

    public bool CanMove(Vector3Int target)
    {
        if (!cells.Contains(target) || visited.Contains(target) || unavailable.Contains(target))
        {
            return false;
        }

        foreach (Vector3Int neighbour in HexPathfinder.GetNeighbours(CurrentCell))
        {
            if (neighbour == target)
            {
                return true;
            }
        }
        return false;
    }

    public void RefreshUnavailableCells()
    {
        HashSet<Vector3Int> blocked = new(visited);
        blocked.Remove(CurrentCell);
        blocked.UnionWith(unavailable);
        HashSet<Vector3Int> usableCells = HexPathfinder.GetCellsOnAnyPath(CurrentCell, Stage.GoalCell, cells, blocked);

        foreach (Vector3Int cell in cells)
        {
            if (!visited.Contains(cell) && !unavailable.Contains(cell) && !usableCells.Contains(cell))
            {
                unavailable.Add(cell);
            }
        }
    }

}
