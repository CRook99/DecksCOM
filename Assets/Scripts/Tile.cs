using System;
using System.Collections;
using System.Collections.Generic;
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
    List<Tile> OrthAdjacencyList;
    List<Tile> DiagAdjacencyList;
    public bool Visited;
    public Tile Parent;
    public float Distance;

    // Position of corners relative to center of tile
    Vector3[] _corners =
    {
        new (0.4f, 0.6f, 0.4f),
        new (-0.4f, 0.6f, 0.4f),
        new (0.4f, 0.6f, -0.4f),
        new (-0.4f, 0.6f, -0.4f)
    };
    
    [SerializeField] GameObject fullShield;
    [SerializeField] GameObject halfShield;
    GameObject cover;
    public bool HasCover;
    public bool HasObstacle;
    Vector3 yOffset = new Vector3(0f, 1.1f, 0f);
    public List<CoverShield> shields;
    
    
    public void Initialize()
    {
        OrthAdjacencyList = new List<Tile>();
        DiagAdjacencyList = new List<Tile>();
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

    public GameObject GetCover() { return cover; }

    public void SetOrthAdjList(List<Tile> list) { OrthAdjacencyList = list; }
    public List<Tile> GetOrthAdjList() { return OrthAdjacencyList; }
    public void SetDiagAdjList(List<Tile> list) { DiagAdjacencyList = list; }
    public List<Tile> GetDiagAdjList() { return DiagAdjacencyList; }


    public void GenerateCoverShields()
    {
        foreach (Tile tile in OrthAdjacencyList)
        {
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
