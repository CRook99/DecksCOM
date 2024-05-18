using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Rendering (colours to be removed)
    Renderer _renderer;
    public Material Debug_Default;
    public Material Debug_Selectable;
    public Material Debug_Current;
    public Material Debug_Edge;

    // BFS
    Dictionary<Vector3, Tile> _orthAdjacencyList = new ();
    Dictionary<Vector3, Tile> _diagAdjacencyList = new (); // Adjacents that can be walked without obstruction
    public bool Visited;
    public Tile Parent;
    public float Distance;
    
    [SerializeField] GameObject fullShield;
    [SerializeField] GameObject halfShield;
    GameObject cover;
    public bool HasCover;
    public bool HasObstacle;
    Vector3 yOffset = new Vector3(0f, 1.1f, 0f);
    public List<CoverShield> shields;
    
    
    public void Initialize()
    {
        TileAdjacencyUtil.ComputeAdjacencyLists(this);

        _renderer = GetComponent<Renderer>();

        FindEnvironment();
    }

    public bool Walkable()
    {
        return !(HasCover || HasObstacle);
    }

    public bool Occupied()
    {
        return Physics.Raycast(transform.position, Vector3.up, out _, 1);
    }

    void FindEnvironment()
    {
        if (!Physics.Raycast(transform.position, Vector3.up, out var hit, 1,
                LayerMask.GetMask("Cover", "Env_Static"))) return;
        
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Cover"))
        {
            cover = hit.collider.gameObject;
            HasCover = true;
        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Env_Static"))
        {
            HasObstacle = true;
        }
    }

    // IMPROVE WHAT THE FUCK
    public void ShowAreaBars()
    {
        
    }

    [ContextMenu("Print neighbours")]
    void PrintNeighbours()
    {
        Debug.Log("ORTHOGONAL");
        foreach (KeyValuePair<Vector3, Tile> kvp in _orthAdjacencyList)
        {
            Debug.Log($"Direction: {kvp.Key} - Neighbour: {kvp.Value}");
        }
        
        Debug.Log("DIAGONAL");
        foreach (KeyValuePair<Vector3, Tile> kvp in _diagAdjacencyList)
        {
            Debug.Log($"Direction: {kvp.Key} - Neighbour: {kvp.Value}");
        }
    }

    public GameObject GetCover() { return cover; }

    public void SetOrthAdjList(Dictionary<Vector3, Tile> list) { _orthAdjacencyList = list; }
    public Dictionary<Vector3, Tile> GetOrthAdjDict() { return _orthAdjacencyList; }
    public List<Tile> GetOrthNeighbours() { return _orthAdjacencyList.Values.ToList(); }
    public void SetDiagAdjList(Dictionary<Vector3, Tile> list) { _diagAdjacencyList = list; }
    public Dictionary<Vector3, Tile> GetDiagAdjDict() { return _orthAdjacencyList; }
    public List<Tile> GetDiagNeighbours() { return _diagAdjacencyList.Values.ToList(); }


    public void GenerateCoverShields()
    {
        foreach (Tile tile in _orthAdjacencyList.Values)
        {
            if (tile == null) continue;
            
            if (tile.GetCover() == null) continue;

            Cover cover = tile.GetCover().GetComponent<Cover>();
            CoverShield shield;
            Vector3 diff = tile.gameObject.transform.position - transform.position;
            switch (cover.GetLevel())
            {
                case CoverLevel.FULL:
                    shield = Instantiate(fullShield, transform.position + (0.4f * diff) + yOffset, Quaternion.LookRotation(diff, Vector3.up), transform).GetComponent<CoverShield>();
                    shields.Add(shield);
                    break;
                case CoverLevel.HALF:
                    shield = Instantiate(halfShield, transform.position + (0.4f * diff) + yOffset, Quaternion.LookRotation(diff, Vector3.up), transform).GetComponent<CoverShield>();
                    shields.Add(shield);
                    break;
            }
        }
    }


    public void ShowCurrent() { _renderer.material = Debug_Current; }

    public void ShowSelectable() { _renderer.material = Debug_Selectable; }

    public void ShowEdge() { _renderer.material = Debug_Edge; }

    public void HideColour() { _renderer.material = Debug_Default; }

    public void ShowShields()
    {
        foreach (CoverShield shield in shields)
        {
            shield.Show();
        }
    }

    public void ShowShields(float scale)
    {
        foreach (CoverShield shield in shields)
        {
            shield.Show(scale);
        }
    }

    public void HideShields()
    {
        foreach (CoverShield shield in shields)
        {
            shield.Hide();
        }
    }

    public void Reset()
    {
        Visited = false;
        Parent = null;
        Distance = 0.0f;
    }
}
