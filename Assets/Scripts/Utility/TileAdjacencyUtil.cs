using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileAdjacencyUtil
{
    //static Tile tile;
    static Vector3 fwd = Vector3.forward;
    static Vector3 rgt = Vector3.right;
    static List<Vector3> orthVectors = new List<Vector3> { fwd, -fwd, rgt, -rgt };
    static List<Vector3> diagVectors = new List<Vector3> { fwd + rgt, fwd - rgt, -fwd + rgt, -fwd - rgt };

    /// <summary>
    /// Freshly computes the adjacency lists of a given tile t
    /// </summary>
    /// <param name="tile"></param>
    public static void ComputeAdjacencyLists(Tile tile)
    {
        tile.Reset();
        tile.SetOrthAdjList(FindOrthNeighbours(tile));
        tile.SetDiagAdjList(FindDiagNeighbours(tile));
    }

    /// <summary>
    /// Finds all neighbouring tiles in orthogonal directions
    /// </summary>
    /// <returns>List of orthogonal neighbours</returns>
    static Dictionary<Vector3, Tile> FindOrthNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();

        foreach (Vector3 v in orthVectors)
        {
            Tile neighbour = GetOrthNeighbour(tile, v);
            neighbours.Add(neighbour);
        }

        return orthVectors.Zip(neighbours, (vector, neighbour) => new { Vector = vector, Neighbour = neighbour })
            .ToDictionary(pair => pair.Vector, pair => pair.Neighbour);
    }

    /// <summary>
    /// Finds all neighbouring tiles in diagonal directions
    /// </summary>
    /// <returns>List of diagonal neighbours</returns>
    static Dictionary<Vector3, Tile> FindDiagNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();

        foreach (Vector3 v in diagVectors)
        {
            Tile neighbour = GetDiagNeighbour(tile, v);
            neighbours.Add(neighbour);
        }
        
        return diagVectors.Zip(neighbours, (vector, neighbour) => new { Vector = vector, Neighbour = neighbour })
            .ToDictionary(pair => pair.Vector, pair => pair.Neighbour);
    }

    /// <summary>
    /// Finds a potential orthogonal neighbour on a given side
    /// </summary>
    /// <param name="direction">The direction to look for a neighbour</param>
    /// <returns>The detected neighbour (nullable)</returns>
    public static Tile GetOrthNeighbour(Tile origin, Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(origin.transform.position + direction, halfExtents);

        foreach (Collider c in colliders)
        {
            Tile t = c.GetComponent<Tile>();
            if (t == null) continue;

            return t;
        }

        return null;
    }

    /// <summary>
    /// Finds a potential diagonal neighbour on a given side
    /// </summary>
    /// <param name="direction">The direction to look for a neighbour</param>
    /// <returns>The detected neighbour (nullable)</returns>
    public static Tile GetDiagNeighbour(Tile origin, Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(origin.transform.position + direction, halfExtents);

        foreach (Collider c in colliders)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile == null) return null;

            Tile xTile = GetOrthNeighbour(origin, new Vector3(direction.x, 0f, 0f));
            Tile zTile = GetOrthNeighbour(origin, new Vector3(0f, 0f, direction.z));

            if ((xTile == null && zTile == null) || !xTile.Walkable() || !zTile.Walkable()) return null; // Add to adj. if not prohibited by lack of tiles/environment

            return tile;
        }

        return null;
    }
}
