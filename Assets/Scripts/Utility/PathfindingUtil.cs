using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PathfindingUtil
{
    public static List<Tile> GetPathToTile(Tile destination)
    {
        if (destination == null) throw new ArgumentNullException();
        List<Tile> path = new List<Tile>();
        Tile current = destination;
        while (current.Parent != null)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Add(current);
        path.Reverse();
        return path;
    }
}
