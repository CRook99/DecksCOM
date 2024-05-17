using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public static class TileOutlineData
{
    static float OFFSET = 0.4f;

    public static Vector3 XPZP = new (OFFSET, 0.6f, OFFSET);
    public static Vector3 XPZN = new (OFFSET, 0.6f, -OFFSET);
    public static Vector3 XNZP = new (-OFFSET, 0.6f, OFFSET);
    public static Vector3 XNZN = new (-OFFSET, 0.6f, -OFFSET);

    public static Vector3[] Corners = new[]
    {
        XPZP,
        XPZN,
        XNZN,
        XNZP
    };

}

public class TileOutline : MonoBehaviour
{
    public static TileOutline Instance { get; private set; }
    float HOR_OFFSET = 0.5f;
    float VER_OFFSET = 0.55f;

    public GameObject BarPrefab;
    [SerializeField] int _poolSize;
    List<GameObject> _bars = new ();
    
    
    List<Vector3> _vertexBuffer;
    List<int> _triBuffer;
    List<int> _subBuffer;

    [SerializeField] Material outer;
    [SerializeField] Material inner;
    Mesh _moveArea;
    MeshFilter _filter;
    MeshRenderer _renderer;

    void Awake()
    {
        Instance = this;
        // _moveArea = new Mesh { subMeshCount = 2 };
        // _filter = GetComponent<MeshFilter>();
        // _filter.mesh = _moveArea;
        // _renderer = GetComponent<MeshRenderer>();
        // _renderer.materials = new []
        // {
        //     inner,
        //     outer
        // };

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

    public GameObject GetBar()
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
        foreach (Tile t in tiles)
        {
            t.ShowAreaBars();
        }
    }

    // public void GenerateMoveAreaMesh(List<Tile> tiles)
    // {
    //     _renderer.enabled = true;
    //     
    //     int numberOfQuads = tiles.Count;
    //
    //     _vertexBuffer = new List<Vector3>(numberOfQuads * 4);
    //     _triBuffer = new List<int>(numberOfQuads * 6);
    //     _subBuffer = new List<int>(numberOfQuads * 6);
    //
    //     foreach (Tile tile in tiles)
    //     {
    //         var pos = tile.transform.position;
    //         
    //         _vertexBuffer.Add(new Vector3(pos.x - HOR_OFFSET, pos.y + VER_OFFSET, pos.z - HOR_OFFSET));
    //         _vertexBuffer.Add(new Vector3(pos.x - HOR_OFFSET, pos.y + VER_OFFSET, pos.z + HOR_OFFSET));
    //         _vertexBuffer.Add(new Vector3(pos.x + HOR_OFFSET, pos.y + VER_OFFSET, pos.z - HOR_OFFSET));
    //         _vertexBuffer.Add(new Vector3(pos.x + HOR_OFFSET, pos.y + VER_OFFSET, pos.z + HOR_OFFSET));
    //     }
    //
    //     for (int i = 0; i < numberOfQuads; i++)
    //     {
    //         _triBuffer.Add(i * 4 + 0);
    //         _triBuffer.Add(i * 4 + 1);
    //         _triBuffer.Add(i * 4 + 2);
    //         _triBuffer.Add(i * 4 + 1);
    //         _triBuffer.Add(i * 4 + 3);
    //         _triBuffer.Add(i * 4 + 2);
    //
    //         _subBuffer.Add(i * 4 + 0);
    //         _subBuffer.Add(i * 4 + 1);
    //         _subBuffer.Add(i * 4 + 2);
    //         _subBuffer.Add(i * 4 + 1);
    //         _subBuffer.Add(i * 4 + 3);
    //         _subBuffer.Add(i * 4 + 2);
    //     }
    //     
    //     _moveArea.Clear();
    //     _moveArea.vertices = _vertexBuffer.ToArray();
    //     _moveArea.SetTriangles(_triBuffer, 0);
    //     _moveArea.SetTriangles(_subBuffer, 1);
    //     _moveArea.RecalculateNormals();
    // }
    //
    // public void HideMoveArea()
    // {
    //     _renderer.enabled = false;
    // }
}
