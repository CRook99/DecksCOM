using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private List<Tile> selectableTiles = new List<Tile>();
    public Tile[] tiles;
    [SerializeField] Tile currentTile;
    private int movementRange;
    private int movementSpeed;

    [SerializeField] protected bool isMoving;
    public Stack<Tile> path;
    private Tile nextTileInPath;
    [SerializeField] private Vector3 nextPosition;
    [SerializeField] private Vector3 directionVector;

    private Vector3 _offset = new Vector3(0f, 0.5f, 0f);

    protected void Initialize()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile").Select(o => o.GetComponent<Tile>()).ToArray();
        ComputeAdjacencyLists();
        isMoving = false;
        path = new Stack<Tile>();
    }

    private void Update()
    {
        if (isMoving)
        {
            Move();
        }
        
    }

    public void SetMovementRange(int range)
    {
        movementRange = range;
    }

    public void SetMovementSpeed(int speed)
    {
        movementSpeed = speed;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.Current = true;
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
        foreach (Tile tile in tiles)
        {
            tile.FindNeighbours();
        }
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
            t.Selectable = true;
            

            if (t.Distance >= movementRange) continue;

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

            t.Visited = true;
        }

        UpdateTileColours();
    }

    public void ResetAllTiles()
    {
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }
        selectableTiles.Clear();
    }


    private void generatePath(Tile destination)
    {
        path.Clear();
        Tile current = destination;
        while (current.Parent != null)
        {
            path.Push(current);
            current = current.Parent;
        }
    }

    public void MoveToDestination(Tile destination)
    {
        isMoving = true;
        CalculateSelectableTiles();
        generatePath(destination);
        CalculateNextPositionInPath();
        CalculateNextDirectionVector();
        
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, nextPosition) >= 0.05f)
        {
            transform.position += movementSpeed * directionVector * Time.deltaTime;
        }
        else
        {
            transform.position = nextPosition;
            CalculateNextPositionInPath();
            CalculateNextDirectionVector();
        }
    }

    private void CalculateNextPositionInPath()
    {
        if (path.Count == 0) // End of path reached
        {
            isMoving = false;
            RecomputeOriginAdjacencies();
            CalculateSelectableTiles();
            return;
        }

        Tile t = path.Pop();
        nextPosition = t.gameObject.transform.position + new Vector3(0f, 0.5f, 0f);
    }

    private void CalculateNextDirectionVector()
    {
        directionVector = (nextPosition - transform.position).normalized;
    }

    private void UpdateTileColours()
    {
        foreach (Tile tile in tiles)
        {
            tile.UpdateColour();
        }
    }

    private void RecomputeOriginAdjacencies()
    {
        foreach (Tile tile in currentTile.OrthAdjacencyList)
        {
            tile.FindNeighbours();
        }

        foreach (Tile tile in currentTile.DiagAdjacencyList)
        {
            tile.FindNeighbours();
        }

        currentTile.FindNeighbours();
    }
}
