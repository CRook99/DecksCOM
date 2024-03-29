using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Rendering (colours to be removed)
    private Renderer _renderer;
    public Material Debug_Default;
    public Material Debug_Selectable;
    public Material Debug_Current;

    // BFS
    private List<Tile> OrthAdjacencyList;
    private List<Tile> DiagAdjacencyList;
    public bool Visited = false;
    public Tile Parent = null;
    public float Distance = 0.0f;

    [SerializeField] GameObject fullShield;
    [SerializeField] GameObject halfShield;
    private GameObject cover;
    private Vector3 yOffset = new Vector3(0f, 1.1f, 0f);
    public List<CoverShield> shields;

    public void Initialize()
    {
        OrthAdjacencyList = new List<Tile>();
        DiagAdjacencyList = new List<Tile>();
        TileAdjacencyUtil.ComputeAdjacencyLists(this);

        _renderer = GetComponent<Renderer>();

        SetCover();
    }

    public bool Walkable()
    {
        return !(Occupied() || GetCover() != null);
    }

    public bool Occupied()
    {
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
