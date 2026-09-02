using System.Collections.Generic;
using UnityEngine;

/// <summary>Hex-grid neighbour lookup and lightweight route queries.</summary>
public static class HexPathfinder
{
    public static IEnumerable<Vector3Int> GetNeighbours(Vector3Int cell)
    {
        yield return new Vector3Int(cell.x - 1, cell.y, cell.z);
        yield return new Vector3Int(cell.x + 1, cell.y, cell.z);

        int diagonalX = (cell.y & 1) == 0 ? -1 : 1;
        yield return new Vector3Int(cell.x, cell.y - 1, cell.z);
        yield return new Vector3Int(cell.x + diagonalX, cell.y - 1, cell.z);
        yield return new Vector3Int(cell.x, cell.y + 1, cell.z);
        yield return new Vector3Int(cell.x + diagonalX, cell.y + 1, cell.z);
    }

    public static bool HasPath(Vector3Int start, Vector3Int goal, HashSet<Vector3Int> cells, HashSet<Vector3Int> blocked)
    {
        if (!cells.Contains(start) || !cells.Contains(goal) || blocked.Contains(start))
        {
            return false;
        }

        Queue<Vector3Int> pending = new();
        HashSet<Vector3Int> visited = new() { start };
        pending.Enqueue(start);
        while (pending.Count > 0)
        {
            Vector3Int current = pending.Dequeue();
            if (current == goal)
            {
                return true;
            }

            foreach (Vector3Int neighbour in GetNeighbours(current))
            {
                if (cells.Contains(neighbour) && !blocked.Contains(neighbour) && visited.Add(neighbour))
                {
                    pending.Enqueue(neighbour);
                }
            }
        }
        return false;
    }

    public static HashSet<Vector3Int> GetCellsOnAnyPath(Vector3Int start, Vector3Int goal, HashSet<Vector3Int> cells, HashSet<Vector3Int> blocked)
    {
        return HexBlockCutTree.GetCellsOnPath(start, goal, cells, blocked);
    }
}
