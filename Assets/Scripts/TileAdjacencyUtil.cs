using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAdjacencyUtil
{
    private static Tile tile;
    private static Vector3 fwd = Vector3.forward;
    private static Vector3 rgt = Vector3.right;
    private static List<Vector3> orthVectors = new List<Vector3> { fwd, -fwd, rgt, -rgt };
    private static List<Vector3> diagVectors = new List<Vector3> { fwd + rgt, fwd - rgt, -fwd + rgt, -fwd - rgt };

    /// <summary>
    /// Freshly computes the adjacency lists of a given tile t
    /// </summary>
    /// <param name="t"></param>
    public static void ComputeAdjacencyLists(Tile t)
    {
        tile = t;
        tile.Reset();
        tile.SetOrthAdjList(FindOrthNeighbours());
        tile.SetDiagAdjList(FindDiagNeighbours());
    }

    /// <summary>
    /// Finds all neighbouring tiles in orthogonal directions
    /// </summary>
    /// <returns>List of orthogonal neighbours</returns>
    private static List<Tile> FindOrthNeighbours()
    {
        List<Tile> neighbours = new List<Tile>();

        foreach (Vector3 v in orthVectors)
        {
            Tile neighbour = GetOrthNeighbour(v);
            if (neighbour != null) neighbours.Add(neighbour);
        }

        return neighbours;
    }

    /// <summary>
    /// Finds all neighbouring tiles in diagonal directions
    /// </summary>
    /// <returns>List of diagonal neighbours</returns>
    private static List<Tile> FindDiagNeighbours()
    {
        List<Tile> neighbours = new List<Tile>();

        foreach (Vector3 v in diagVectors)
        {
            Tile neighbour = GetDiagNeighbour(v);
            if (neighbour != null) neighbours.Add(neighbour);
        }

        return neighbours;
    }

    /// <summary>
    /// Finds a potential orthogonal neighbour on a given side
    /// </summary>
    /// <param name="direction">The direction to look for a neighbour</param>
    /// <returns>The detected neighbour (nullable)</returns>
    private static Tile GetOrthNeighbour(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(tile.transform.position + direction, halfExtents);

        foreach (Collider c in colliders)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile == null) continue;

            return tile;
        }

        return null;
    }

    /// <summary>
    /// Finds a potential diagonal neighbour on a given side
    /// </summary>
    /// <param name="direction">The direction to look for a neighbour</param>
    /// <returns>The detected neighbour (nullable)</returns>
    private static Tile GetDiagNeighbour(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(tile.transform.position + direction, halfExtents);

        foreach (Collider c in colliders)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile == null) return null;

            Tile xTile = GetOrthNeighbour(new Vector3(direction.x, 0f, 0f));
            Tile zTile = GetOrthNeighbour(new Vector3(0f, 0f, direction.z));

            if ((xTile == null && zTile == null) || !xTile.Walkable() || !zTile.Walkable()) continue; // Add to adj. if not prohibited by lack of tiles/environment

            return tile;
        }

        return null;
    }
}
