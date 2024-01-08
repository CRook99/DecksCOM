using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] protected List<Tile> selectableTiles;
    protected Tile currentTile;
    private int movementRange;
    private int movementSpeed = 5;

    [SerializeField] protected bool isMoving;

    private Vector3 _offset = new Vector3(0f, 0.5f, 0f);

    void Awake()
    {
        selectableTiles = new List<Tile>();
        isMoving = false;
    }

    private void Update()
    {

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

            if (t.Distance >= movementRange) continue;

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
    }

    public void ShowRange()
    {
        foreach (Tile tile in selectableTiles) { tile.ShowSelectable();}
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
    }


    private Stack<Tile> generatePath(Tile destination)
    {
        Stack<Tile> path = new Stack<Tile>();
        Tile current = destination;
        while (current.Parent != null)
        {
            path.Push(current);
            current = current.Parent;
        }

        return path;
    }

    public IEnumerator MoveToDestination(Tile destination)
    {
        isMoving = true;
        HideRange();
        Stack<Tile> path = generatePath(destination);
        Vector3 startPosition, nextPosition, directionVector;
        
        while (path.Count > 0)
        {
            startPosition = transform.position;
            nextPosition = path.Pop().gameObject.transform.position + _offset;
            directionVector = (nextPosition - startPosition).normalized;

            while (Vector3.Distance(transform.position, nextPosition) >= 0.05f)
            {
                transform.position += movementSpeed * directionVector * Time.deltaTime;
                yield return null;
            }

            transform.position = nextPosition;
        }

        isMoving = false;
        
        // extra
        TeamSwitcher.Instance.Enable();
        yield break;
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
