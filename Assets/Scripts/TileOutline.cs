using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOutline : MonoBehaviour
{
    public static TileOutline Instance { get; private set; }
    public static float HOR_OFFSET = 0.5f;
    public static float VER_OFFSET = 0.55f;

    public GameObject BarPrefab;
    [SerializeField] int _poolSize;
    List<GameObject> _bars = new ();
    

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < _poolSize; i++)
        {
            CreateBar();
        }
    }

    GameObject CreateBar()
    {
        GameObject bar = Instantiate(BarPrefab, Vector3.zero, Quaternion.identity, transform);
        bar.SetActive(false);
        _bars.Add(bar);
        return bar;
    }

    GameObject GetBar()
    {
        GameObject bar = _bars.Find(b => !b.activeInHierarchy);
        if (bar == null)
        {
            bar = CreateBar();
        }
        bar.SetActive(true);
        return bar;
    }

    public void ShowOutline(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            foreach (var (dir, neighbour) in tile.GetOrthAdjDict())
            {
                if (neighbour != null && neighbour.Visited && neighbour.Walkable()) continue;
                
                GameObject bar = GetBar();
                bar.transform.SetPositionAndRotation(tile.transform.position + dir * HOR_OFFSET + Vector3.up * VER_OFFSET,
                    Quaternion.Euler(0, 90 * (dir.z != 0 ? 1 : 0), 0)); // Rotate by 90 if z is not 0
            }
        }
    }

    public void HideOutline()
    {
        foreach (GameObject bar in _bars)
        {
            bar.SetActive(false);
        }
    }
}
