using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Static class that offers utility functions for pathfinding.
/// </summary>
public static class Pathfinding {
    private static readonly IList<Vector2Int> AdjacencyArray = new List<Vector2Int> {
        new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1)
    };

    /// <summary>
    /// Returns an array of 2D coordinates that represents an optimal path between two points.
    /// </summary>
    /// <remarks>
    /// Uses euclidian distance as a heuristic function. Read this for more details:
    /// https://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
    /// </remarks>
    /// <param name="grid">A map of points on a grid, along with the validity of that point.</param>
    /// <param name="start">The start tile coordinates.</param>
    /// <param name="end">The end tile coordinates.</param>
    /// <param name="allowDiagonal">Whether or not to consider diagonal moves valid.</param>
    public static Vector2Int[] FindShortestPath(IEnumerable<PathingTile> grid, Vector2Int start, Vector2Int end, bool allowDiagonal = false) {
        var gridMap = new Dictionary<Vector2Int, PathingTile>();
        foreach (var element in grid) {
            gridMap[element.Position] = element;
        }
        var endNode = new Node(end);

        var openList = new List<Node> {
            new Node(start)
        };
        var closedList = new List<Node>();

        while (openList.Count > 0) {
            var currentNode = openList[0];
            var currentIndex = 0;
            for (var i = 0; i < openList.Count; i++) {
                if (openList[i].Heuristics.F < currentNode.Heuristics.F) {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }
            openList.RemoveAt(currentIndex);
            closedList.Add(currentNode);

            if (currentNode.Position == end) {
                var path = new Stack<Vector2Int>();
                var current = currentNode;
                while (current != null) {
                    path.Push(current.Position);
                    current = current.Parent;
                }
                return path.ToArray();
            }

            var children = new List<Node>();
            foreach (var newPosition in AdjacencyArray) {
                if (!allowDiagonal && (Mathf.Abs(newPosition.x) + Mathf.Abs(newPosition.y)) > 1) {
                    continue;
                }
                var nodePosition = currentNode.Position + newPosition;
                if (gridMap.TryGetValue(nodePosition, out PathingTile selector) && selector.Traversable) {
                    children.Add(new Node(nodePosition, currentNode));
                }
            }
            foreach (var child in children) {
                var abandonChild = false;
                foreach (var closedChild in closedList) {
                    if (child.Position == closedChild.Position) {
                        abandonChild = true;
                        break;
                    }
                }
                child.Heuristics.G = currentNode.Heuristics.G + 1;
                child.Heuristics.H = ((child.Position.x - endNode.Position.x) ^ 2) + ((child.Position.y - endNode.Position.y) ^ 2);
                child.Heuristics.F = child.Heuristics.G + child.Heuristics.H;
                foreach (var openNode in openList) {
                    if (child.Position == openNode.Position && child.Heuristics.G > openNode.Heuristics.G) {
                        abandonChild = true;
                        break;
                    }
                }
                if (!abandonChild) {
                    openList.Add(child);
                }
            }
        }
        return new Vector2Int[0];
    }
}


public class Node {
    public Heuristics Heuristics { get; set; }
    public Vector2Int Position { get; set; }
    public Node Parent { get; set; }

    public Node(Vector2Int position, Node parent = null, Heuristics heuristics = null) {
        Position = position;
        Parent = parent;
        Heuristics = heuristics ?? new Heuristics();
    }
}

public class Heuristics {
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Heuristics(int g = 0, int h = 0, int f = 0) {
        G = g;
        H = h;
        F = f;
    }
}

[DebuggerDisplay("{Position}: {Traversable}")]
public readonly struct PathingTile {
    public Vector2Int Position { get; }
    public bool Traversable { get; }

    public PathingTile(Vector2Int position, bool traversable) {
        Position = position;
        Traversable = traversable;
    }
}