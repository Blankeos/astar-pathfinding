using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Vector3Int Pos { get; set; }

    // Deferred Properties
    public List<Node> Neighbors { get; protected set; }

    // Pathfinding Properties (Changes everytime Pathfinding will be called)
    public bool Walkable { get; set; } = true;
    public Node Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public Node(Vector3Int pos)
    {
        Pos = pos;
    }

    private static readonly List<Vector3Int> Dirs = new List<Vector3Int>() {
        new Vector3Int(0, 1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(1, 0, 0)
    };

    public void CacheNeighbors()
    {
        Neighbors = new List<Node>();

        foreach (Node node in Dirs.Select(dir => GridManager.Instance.GetNodeAtPosition(Pos + dir)).Where(node => node != null))
        {
            Neighbors.Add(node);
        }
    }

    public void SetConnection(Node node)
    {
        Connection = node;
    }

    public void SetG(float g)
    {
        G = g;
    }

    public void SetH(float h)
    {
        H = h;
    }

    public float GetDistance(Node other)
    {
        var dist = new Vector3Int(Mathf.Abs((int)Pos.x - (int)other.Pos.x), Mathf.Abs((int)Pos.y - (int)other.Pos.y), 0);

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }

    public override string ToString()
    {
        return $"<Node: {Pos.x},{Pos.y},{Pos.z}>";
    }
}