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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) UpdateColour();
    }

    private void Start()
    {
        OrthAdjacencyList = new List<Tile>();
        DiagAdjacencyList = new List<Tile>();

        _renderer = GetComponent<Renderer>();
    }

    // Can describe a map tile occupied by an entity or environment block
    public bool Occupied()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.yellow, 5f);
        return Physics.Raycast(transform.position, Vector3.up, out _, 1);
    }


    [ContextMenu("Log Occupied")]
    public void LogOccupied()
    {
        Debug.Log(Occupied());
    }

    public void FindNeighbours()
    {
        Reset();

        CheckNeighbour(Vector3.forward, OrthAdjacencyList);
        CheckNeighbour(-Vector3.forward, OrthAdjacencyList);
        CheckNeighbour(Vector3.right, OrthAdjacencyList);
        CheckNeighbour(-Vector3.right, OrthAdjacencyList);

        CheckNeighbour(Vector3.forward + Vector3.right, DiagAdjacencyList);
        CheckNeighbour(Vector3.forward - Vector3.right, DiagAdjacencyList);
        CheckNeighbour(-Vector3.forward + Vector3.right, DiagAdjacencyList);
        CheckNeighbour(-Vector3.forward - Vector3.right, DiagAdjacencyList);
    }

    private void CheckNeighbour(Vector3 direction, List<Tile> list)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider c in colliders)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile != null && !tile.Occupied())
            {
                list.Add(tile);
            }
        }
    }

    private void UpdateColour()
    {
        if (Current) _renderer.material = Debug_Current;
        else if (Selectable) _renderer.material = Debug_Selectable;
        else _renderer.material = Debug_Default;
    }

    private void Reset()
    {
        Visited = false;
        Parent = null;
        Distance = 0.0f;
    }
}
