using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool Current = false;
    public bool Selectable = false;

    // Rendering (colours to be removed)
    private Renderer _renderer;
    public Material Debug_Default;
    public Material Debug_Selectable;
    public Material Debug_Current;

    // BFS
    public List<Tile> OrthAdjacencyList = new List<Tile>();
    public List<Tile> DiagAdjacencyList = new List<Tile>();
    public bool Visited = false;
    public Tile Parent = null;
    public float Distance = 0.0f;

    [SerializeField] GameObject cover;
    [SerializeField] GameObject fullShield;
    [SerializeField] GameObject halfShield;
    private Vector3 yOffset = new Vector3(0f, 1.1f, 0f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) UpdateColour();
    }

    private void Start()
    {
        OrthAdjacencyList = new List<Tile>();
        DiagAdjacencyList = new List<Tile>();

        SetCover();

        _renderer = GetComponent<Renderer>();
    }

    public bool Walkable()
    {
        return !(Occupied() || cover != null);
    }

    // Can describe a map tile occupied by an entity or environment block
    public bool Occupied()
    {
        //Debug.DrawRay(transform.position, Vector3.up, Color.yellow, 5f);
        return Physics.Raycast(transform.position, Vector3.up, out _, 1);
    }

    public bool SetCover()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1, LayerMask.GetMask("Cover")))
        {
            cover = hit.collider.gameObject;
            return true;
        }

        return false;
    }

    public GameObject GetCover()
    {
        return cover;
    }


    [ContextMenu("Log Occupied")]
    public void LogOccupied()
    {
        Debug.Log(Occupied());
    }

    [ContextMenu("Log Get Cover")]
    public void LogGetCover()
    {
        Debug.Log(GetCover());
    }

    public void FindNeighbours()
    {
        Reset();

        

        AddOrthNeighbour(GetOrthNeighbour(Vector3.forward));
        AddOrthNeighbour(GetOrthNeighbour(-Vector3.forward));
        AddOrthNeighbour(GetOrthNeighbour(Vector3.right));
        AddOrthNeighbour(GetOrthNeighbour(-Vector3.right));

        AddDiagNeighbour(GetDiagNeighbour(Vector3.forward, Vector3.right));
        AddDiagNeighbour(GetDiagNeighbour(Vector3.forward, -Vector3.right));
        AddDiagNeighbour(GetDiagNeighbour(-Vector3.forward, Vector3.right));
        AddDiagNeighbour(GetDiagNeighbour(-Vector3.forward, -Vector3.right));

        if (GetCover() == null) GenerateCoverShields();
    }

    private Tile GetOrthNeighbour(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider c in colliders)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile == null) continue;
                            
            return tile;
        }

        return null;
    }

    private Tile GetDiagNeighbour(Vector3 xDirection, Vector3 zDirection)
    { 
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + (xDirection + zDirection), halfExtents);

        foreach (Collider c in colliders)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile == null) return null;

            Tile xTile = GetOrthNeighbour(xDirection);
            Tile zTile = GetOrthNeighbour(zDirection);

            if ((xTile == null && zTile == null) || !xTile.Walkable() || !zTile.Walkable()) continue;

            return tile;
        }

        return null;
    }

    private void AddOrthNeighbour(Tile t)
    {
        if (t != null) OrthAdjacencyList.Add(t);
    }

    private void AddDiagNeighbour(Tile t)
    {
        if (t != null) DiagAdjacencyList.Add(t);
    }

    private void GenerateCoverShields()
    {
        foreach (Tile tile in OrthAdjacencyList)
        {
            if (tile.GetCover() == null) continue;

            Cover cover = tile.GetCover().GetComponent<Cover>();
            Vector3 diff = tile.gameObject.transform.position - transform.position;
            switch (cover.GetLevel())
            {
                case CoverLevel.FULL:
                    Instantiate(fullShield, transform.position + (0.4f * diff) + yOffset, Quaternion.LookRotation(diff, Vector3.up), transform);
                    break;
                case CoverLevel.HALF:
                    Instantiate(halfShield, transform.position + (0.4f * diff) + yOffset, Quaternion.LookRotation(diff, Vector3.up), transform);
                    break;
            }
        }
    }

    public void UpdateColour()
    {
        if (Current) _renderer.material = Debug_Current;
        else if (Selectable) _renderer.material = Debug_Selectable;
        else _renderer.material = Debug_Default;
    }

    public void Reset()
    {
        Visited = false;
        Parent = null;
        Distance = 0.0f;

        Current = false;
        Selectable = false;
    }
}
