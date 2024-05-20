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
    
    [SerializeField] List<Tile> selectableTiles;
    [SerializeField] List<Tile> edgeTiles;
    Tile currentTile;

    [SerializeField] int _movementRange;
    [SerializeField] int _movementSpeed;
    int bonus;

    Vector3 _offset = new (0f, 0.5f, 0f);

    void Awake()
    {
        selectableTiles = new List<Tile>();
    }

    public void SetMovementRange(int range)
    {
        _movementRange = range;
    }

    public int GetMovementRange()
    {
        return _movementRange;
    }

    public void IncrementBonus()
    {
        bonus++;
    }

    public void ResetBonus()
    {
        bonus = 0;
    }
    

    public Tile GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        return currentTile;
    }

    Tile GetTargetTile(GameObject target)
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

        selectableTiles = PathfindingUtil.FindReachableTiles(currentTile, _movementRange + bonus);
    }

    bool IsEdgeTile(Tile tile)
    {
        foreach (Tile t in tile.GetOrthNeighbours())
        {
            if (!t.Visited) return true;
        }
        
        foreach (Tile t in tile.GetDiagNeighbours())
        {
            if (!t.Visited) return true;
        }

        return false;
    }

    public void ShowRange()
    {
        MovementOutline.Instance.SetArea(selectableTiles);
    }

    public void HideRange()
    {
        MovementOutline.Instance.HideOutline();
    }

    public List<Tile> GetReachableTiles()
    {
        return selectableTiles;
    }

    public void ResetAllTiles()
    {
        selectableTiles.Clear();
        edgeTiles.Clear();
        TileManager.Instance.ResetAllTiles();
    }
    

    public IEnumerator MoveToDestination(Tile destination)
    {
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
                transform.position += directionVector * (_movementSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = nextPosition;
        }
        
        OnEndMove?.Invoke();
    }

    private void RecomputeOriginAdjacencies()
    {
        foreach (Tile tile in currentTile.GetOrthNeighbours())
        {
            TileAdjacencyUtil.ComputeAdjacencyLists(tile);
        }

        foreach (Tile tile in currentTile.GetDiagNeighbours())
        {
            TileAdjacencyUtil.ComputeAdjacencyLists(tile);
        }

        TileAdjacencyUtil.ComputeAdjacencyLists(currentTile);
    }
}
