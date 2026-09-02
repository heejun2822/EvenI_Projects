using System.Collections.Generic;
using UnityEngine;

/// <summary>Finds cells that can appear on at least one simple start-to-goal path.</summary>
internal static class HexBlockCutTree
{
    public static HashSet<Vector3Int> GetCellsOnPath(Vector3Int start, Vector3Int goal, HashSet<Vector3Int> cells, HashSet<Vector3Int> blocked)
    {
        HashSet<Vector3Int> available = new(cells);
        available.ExceptWith(blocked);
        if (!available.Contains(start) || !available.Contains(goal))
        {
            return new HashSet<Vector3Int>();
        }

        BiconnectedComponentFinder finder = new(available);
        List<HashSet<Vector3Int>> components = finder.FindFrom(start);
        if (!finder.DiscoveryOrder.ContainsKey(goal))
        {
            return new HashSet<Vector3Int>();
        }

        return BuildPathCells(start, goal, finder.DiscoveryOrder.Keys, components);
    }

    private static HashSet<Vector3Int> BuildPathCells(Vector3Int start, Vector3Int goal, Dictionary<Vector3Int, int>.KeyCollection cells,
        List<HashSet<Vector3Int>> components)
    {
        // The block-cut graph is a tree: only blocks on the start-to-goal branch can contribute valid cells.
        List<Vector3Int> vertices = new(cells);
        Dictionary<Vector3Int, int> vertexIndices = new();
        for (int index = 0; index < vertices.Count; index++)
        {
            vertexIndices.Add(vertices[index], index);
        }

        List<int>[] graph = CreateBlockCutGraph(vertices.Count, components, vertexIndices);
        int startNode = vertexIndices[start];
        int goalNode = vertexIndices[goal];
        int[] parents = FindParents(graph, startNode, goalNode);
        if (parents[goalNode] == -1)
        {
            return new HashSet<Vector3Int>();
        }

        HashSet<Vector3Int> result = new();
        for (int node = goalNode; node != startNode; node = parents[node])
        {
            AddNodeCells(node, vertices, components, result);
        }
        AddNodeCells(startNode, vertices, components, result);
        return result;
    }

    private static List<int>[] CreateBlockCutGraph(int vertexCount, List<HashSet<Vector3Int>> components, Dictionary<Vector3Int, int> vertexIndices)
    {
        List<int>[] graph = new List<int>[vertexCount + components.Count];
        for (int index = 0; index < graph.Length; index++)
        {
            graph[index] = new List<int>();
        }

        for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
        {
            int componentNode = vertexCount + componentIndex;
            foreach (Vector3Int cell in components[componentIndex])
            {
                int vertexNode = vertexIndices[cell];
                graph[vertexNode].Add(componentNode);
                graph[componentNode].Add(vertexNode);
            }
        }
        return graph;
    }

    private static int[] FindParents(List<int>[] graph, int startNode, int goalNode)
    {
        int[] parents = new int[graph.Length];
        for (int index = 0; index < parents.Length; index++)
        {
            parents[index] = -1;
        }

        Queue<int> pending = new();
        parents[startNode] = startNode;
        pending.Enqueue(startNode);
        while (pending.Count > 0 && parents[goalNode] == -1)
        {
            int current = pending.Dequeue();
            foreach (int neighbour in graph[current])
            {
                if (parents[neighbour] == -1)
                {
                    parents[neighbour] = current;
                    pending.Enqueue(neighbour);
                }
            }
        }
        return parents;
    }

    private static void AddNodeCells(int node, List<Vector3Int> vertices, List<HashSet<Vector3Int>> components, HashSet<Vector3Int> result)
    {
        if (node < vertices.Count)
        {
            result.Add(vertices[node]);
            return;
        }
        result.UnionWith(components[node - vertices.Count]);
    }

    private sealed class BiconnectedComponentFinder
    {
        private readonly HashSet<Vector3Int> available;
        private readonly Dictionary<Vector3Int, int> lowLinks = new();
        private readonly Stack<Edge> edges = new();
        private readonly List<HashSet<Vector3Int>> components = new();
        private int time;

        public Dictionary<Vector3Int, int> DiscoveryOrder { get; } = new();

        public BiconnectedComponentFinder(HashSet<Vector3Int> available)
        {
            this.available = available;
        }

        public List<HashSet<Vector3Int>> FindFrom(Vector3Int start)
        {
            Visit(start, start);
            return components;
        }

        private void Visit(Vector3Int current, Vector3Int parent)
        {
            DiscoveryOrder[current] = ++time;
            lowLinks[current] = time;
            foreach (Vector3Int neighbour in HexPathfinder.GetNeighbours(current))
            {
                if (!available.Contains(neighbour))
                {
                    continue;
                }

                if (!DiscoveryOrder.ContainsKey(neighbour))
                {
                    edges.Push(new Edge(current, neighbour));
                    Visit(neighbour, current);
                    lowLinks[current] = Mathf.Min(lowLinks[current], lowLinks[neighbour]);
                    if (lowLinks[neighbour] >= DiscoveryOrder[current])
                    {
                        AddComponent(current, neighbour);
                    }
                }
                else if (neighbour != parent && DiscoveryOrder[neighbour] < DiscoveryOrder[current])
                {
                    edges.Push(new Edge(current, neighbour));
                    lowLinks[current] = Mathf.Min(lowLinks[current], DiscoveryOrder[neighbour]);
                }
            }
        }

        private void AddComponent(Vector3Int from, Vector3Int to)
        {
            HashSet<Vector3Int> component = new();
            Edge edge;
            do
            {
                edge = edges.Pop();
                component.Add(edge.from);
                component.Add(edge.to);
            }
            while (edge.from != from || edge.to != to);
            components.Add(component);
        }
    }

    private readonly struct Edge
    {
        public readonly Vector3Int from;
        public readonly Vector3Int to;

        public Edge(Vector3Int from, Vector3Int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
