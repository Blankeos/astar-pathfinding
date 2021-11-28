using System.Collections.Generic;
using UnityEngine;

public static class GridUtility
{
    public static Dictionary<Vector3Int, Node> Generate(int xMin, int xMax, int yMin, int yMax)
    {
        var tiles = new Dictionary<Vector3Int, Node>();

        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Node tile = new Node(pos);
                tiles.Add(pos, tile);
            }
        }

        return tiles;
    }
}
