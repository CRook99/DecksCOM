using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GridMovement : MonoBehaviour
{
    public static event Action OnEndMove;
    
    [SerializeField] protected List<Tile> selectableTiles;
    [SerializeField] protected List<Tile> edgeTiles;
    protected Tile currentTile;
    
    [SerializeField] int _movementRange;
    [SerializeField] int _movementSpeed;
    int bonus;
    
    [SerializeField] protected bool isMoving;

    private Vector3 _offset = new Vector3(0f, 0.5f, 0f);

    void Awake()
    {
        selectableTiles = new List<Tile>();
        isMoving = false;
    }

    public void SetMovementRange(int range)
    {
        _movementRange = range;
    }
    
    public void SetMovementSpeed(int speed)
    {
        _movementSpeed = speed;
    }

    public void IncrementBonus()
    {
        bonus++;
    }

    public void ResetBonus()
    {
        bonus = 0;
    }
    

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
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

    public void CalculateSelectableTiles()
    {
        ResetAllTiles();
        GetCurrentTile();

        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(currentTile);

        while (queue.Count > 0)
        {
            Tile t = queue.Dequeue();
            selectableTiles.Add(t);

            if (t.Distance >= _movementRange + bonus) continue;

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
        
        edgeTiles = selectableTiles.Where(IsEdgeTile).ToList();
    }

    bool IsEdgeTile(Tile tile)
    {
        foreach (Tile t in tile.GetOrthAdjList())
        {
            if (!t.Visited) return true;
        }
        
        foreach (Tile t in tile.GetDiagAdjList())
        {
            if (!t.Visited) return true;
        }

        return false;
    }

    public void ShowRange()
    {
        foreach (Tile tile in selectableTiles) { tile.ShowSelectable();}
        foreach (Tile tile in edgeTiles) { tile.ShowEdge(); }
        currentTile.ShowCurrent();
    }

    public void HideRange()
    {
        foreach (Tile tile in selectableTiles) { tile.HideColour(); }
    }

    public void ResetAllTiles()
    {
        if (selectableTiles == null) return;
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }
        selectableTiles.Clear();
        edgeTiles.Clear();
    }
    

    public IEnumerator MoveToDestination(Tile destination)
    {
        isMoving = true;
        HideRange();
        List<Tile> path = PathfindingUtil.GetPathToTile(destination);
        Vector3 startPosition, nextPosition, directionVector;
        
        for (int i = 0; i < path.Count; i++)
        {
            startPosition = transform.position;
            nextPosition = path[i].gameObject.transform.position + _offset;
            directionVector = (nextPosition - startPosition).normalized;

            while (Vector3.Distance(transform.position, nextPosition) >= 0.05f)
            {
                transform.position += _movementSpeed * directionVector * Time.deltaTime;
                yield return null;
            }

            transform.position = nextPosition;
        }

        isMoving = false;
        
        OnEndMove?.Invoke();
    }

    private void RecomputeOriginAdjacencies()
    {
        foreach (Tile tile in currentTile.GetOrthAdjList())
        {
            TileAdjacencyUtil.ComputeAdjacencyLists(tile);
        }

        foreach (Tile tile in currentTile.GetDiagAdjList())
        {
            TileAdjacencyUtil.ComputeAdjacencyLists(tile);
        }

        TileAdjacencyUtil.ComputeAdjacencyLists(currentTile);
    }
}
