using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private List<Tile> _selectableTiles = new List<Tile>();
    public GameObject[] _tiles;
    private Tile _currentTile;
    protected int _movement;

    private Vector3 _offset = new Vector3(0f, 0.5f, 0f);

    protected void Initialize()
    {
        _tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    public void SetMovement(int movement)
    {
        _movement = movement;
    }

    public void GetCurrentTile()
    {
        _currentTile = GetTargetTile(gameObject);
        _currentTile.Current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position + _offset, -Vector3.up, out hit, 1f))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        foreach (GameObject tile in _tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbours();
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(_currentTile);

        while (queue.Count > 0)
        {
            Tile t = queue.Dequeue();
            _selectableTiles.Add(t);
            t.Selectable = true;

            if (t.Distance >= _movement) continue;

            foreach (Tile a in t.OrthAdjacencyList)
            {
                if (a.Visited) continue;
                
                a.Parent = t;
                a.Visited = true;
                a.Distance = 1 + t.Distance;
                queue.Enqueue(a);
                
            }

            foreach (Tile a in t.DiagAdjacencyList)
            {
                if (a.Visited) continue;

                a.Parent = t;
                a.Visited = true;
                a.Distance = Mathf.Sqrt(2) + t.Distance;
                queue.Enqueue(a);
            }


        }
    }
}
