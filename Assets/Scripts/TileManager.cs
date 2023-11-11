using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static Tile[] tiles;
    private static TileManager _instance;
    public static TileManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
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
}
