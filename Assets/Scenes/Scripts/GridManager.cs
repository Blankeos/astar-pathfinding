using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    void Awake() => Instance = this;

    public Grid Grid;
    public Vector2Int MinBounds;
    public Vector2Int MaxBounds;

    public Dictionary<Vector3Int, Node> Nodes;

    private void Start()
    {
        if (Grid == null) Grid = GetComponent<Grid>();
        Nodes = GridUtility.Generate(MinBounds.x, MaxBounds.x, MinBounds.y, MaxBounds.y);
        foreach (Node node in Nodes.Values)
        {
            bool walkable = Random.Range(0f, 1f) > 0.8 ? false : true;
            node.Walkable = walkable;
            node.CacheNeighbors();
        }
    }

    public Node GetNodeAtPosition(Vector3Int pos) => Nodes.TryGetValue(pos, out var node) ? node : null;

    // Temporary Pathfinding Input
    private Node _startNode;
    private Node _targetNode;
    private List<Node> _paths;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cellPos = Grid.WorldToCell(worldPosition);
            var node = GetNodeAtPosition(cellPos);

            if (Input.GetMouseButtonDown(0) && node != null && node.Walkable)
            {
                _startNode = node;
            }
            else if (Input.GetMouseButtonDown(1) && node != null && node.Walkable)
            {
                _targetNode = GetNodeAtPosition(cellPos);
            }

            if (Nodes.ContainsKey(cellPos) && _startNode != null && _targetNode != null)
            {
                _paths = Pathfinding.FindPath(_startNode, _targetNode);
            }
        }
    }

    // Debugging
    private void OnDrawGizmos()
    {
        // Debug Preview Grid Bounds
        if (!Application.isPlaying)
        {
            Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.3f);
            for (int x = MinBounds.x; x < MaxBounds.x; x++)
            {
                for (int y = MinBounds.y; y < MaxBounds.y; y++)
                {
                    Gizmos.DrawCube(new Vector3(x + Grid.cellSize.x * 0.5f, y + Grid.cellSize.y * 0.5f, 0), new Vector3(0.8f, 0.8f, 0));
                }
            }
        }

        // Debug Actual Play Mode Bounds
        if (Nodes == null) return;
        foreach (var node in Nodes.Values)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Grid.WorldToCell(worldPosition) == node.Pos)
            {
                Gizmos.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0.3f);
            }
            else
            {
                Gizmos.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.3f);
            }
            if (_paths != null)
            {
                if (_paths.Contains(node))
                {
                    Gizmos.color = Color.blue;
                }
            }
            if (!node.Walkable)
            {
                Gizmos.color = Color.gray;
            }

            if (node == _startNode)
            {
                Gizmos.color = Color.green;
            }
            if (node == _targetNode)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawCube(new Vector3(node.Pos.x + Grid.cellSize.x * 0.5f, node.Pos.y + Grid.cellSize.y * 0.5f, 0), new Vector3(0.9f, 0.9f, 0));
        }
    }
}