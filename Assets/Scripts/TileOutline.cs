using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileOutlineData
{
    static float OFFSET = 0.4f;

    public static Vector3 XPZP = new (OFFSET, 0.6f, OFFSET);
    public static Vector3 XPZN = new (OFFSET, 0.6f, -OFFSET);
    public static Vector3 XNZP = new (-OFFSET, 0.6f, OFFSET);
    public static Vector3 XNZN = new (-OFFSET, 0.6f, -OFFSET);

    public static Vector3[] Corners = new[]
    {
        XPZP,
        XPZN,
        XNZN,
        XNZP
    };

}

public class TileOutline : MonoBehaviour
{
    public static TileOutline Instance { get; private set; }

    LineRenderer _line;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            var pos = tile.transform.position;
            
            // X-positive
            

            // X-negative


            // Z-positive


            // Z-negative
        }
    }
}
