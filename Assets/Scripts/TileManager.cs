using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    static Tile[] tiles;
    public static TileManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        tiles = GameObject.FindGameObjectsWithTag("Tile").Select(o => o.GetComponent<Tile>()).ToArray();
        foreach (Tile tile in tiles)
        {
            tile.Initialize();
        }

        foreach (Tile tile in tiles)
        {
            if (tile.GetCover() == null) tile.GenerateCoverShields();
        }
    }

    public Tile[] GetAllTiles()
    {
        return tiles;
    }

    public void ResetAllTiles()
    {
        foreach (Tile tile in tiles)
        {
            tile.Reset();
        }
    }
}
