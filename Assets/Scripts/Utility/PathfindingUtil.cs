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

    public static List<Tile> FindReachableTiles(Tile origin, int range)
    {
        List<Tile> reachableTiles = new List<Tile>();
        
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(origin);

        while (queue.Count > 0)
        {
            Tile t = queue.Dequeue();
            reachableTiles.Add(t);

            if (t.Distance >= range) continue;

            foreach (Tile a in t.GetOrthAdjList())
            {
                if (a.Visited || !a.Walkable()) continue;
                
                a.Parent = t;
                a.Visited = true;
                a.Distance = 1 + t.Distance;
                queue.Enqueue(a);
                
            }

            foreach (Tile a in t.GetDiagAdjList())
            {
                if (a.Visited || !a.Walkable()) continue;

                a.Parent = t;
                a.Visited = true;
                a.Distance = Mathf.Sqrt(2) + t.Distance;
                queue.Enqueue(a);
            }

            t.Visited = true;
        }

        return reachableTiles;
    }
}
