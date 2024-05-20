using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileOutliner : MonoBehaviour
{
    public static float HOR_OFFSET = 0.5f;
    public static float VER_OFFSET = 0.55f;
    public static float BASE_VER_OFFSET = 0.501f;

    public bool DrawOutline;
    public bool DrawArea;

    List<Tile> _tiles;
    
    public GameObject Prefab;
    int _poolSize = 200;
    //List<GameObject> _objects;
    HashSet<OutlineObject> _objects;
    
    public Material BaseMaterial;
    Mesh _area;
    MeshFilter _filter;
    MeshRenderer _renderer;
    List<Vector3> _vertexBuffer;
    List<int> _triBuffer;
    List<Vector2> _uvs;
    List<int> _subBuffer;

    IOutlineDecisionStrategy _strategy;

    protected virtual void Awake()
    {
        if (DrawOutline)
        {
            _objects = new HashSet<OutlineObject>();
            for (int i = 0; i < _poolSize; i++)
            {
                CreateObject();
            }
        }

        if (DrawArea)
        {
            _area = new Mesh();
            _filter = GetComponent<MeshFilter>();
            _filter.mesh = _area;
            _renderer = GetComponent<MeshRenderer>();
            _renderer.sharedMaterial = BaseMaterial;
        }
    }

    public void SetDecisionStrategy(IOutlineDecisionStrategy strategy)
    {
        if (strategy == null) return;
        
        _strategy = strategy;
    }
    
    OutlineObject CreateObject()
    {
        GameObject go = Instantiate(Prefab, Vector3.zero, Quaternion.identity, transform);
        OutlineObject obj = new OutlineObject(go);
        _objects.Add(obj);
        obj.Disable();
        return obj;
    }
    
    OutlineObject GetObject()
    {
        OutlineObject obj = _objects.FirstOrDefault(o => !o.InUse);
        if (obj == null) obj = CreateObject();
        obj.Enable();
        return obj;
        
        // GameObject obj = _objects.Find(b => !b.activeInHierarchy);
        // if (obj == null)
        // {
        //     obj = CreateObject();
        // }
        // obj.SetActive(true);
        // return obj;
    }

    [ContextMenu("Print all objects")]
    void PrintObjects()
    {
        int i = 0;
        foreach (OutlineObject o in _objects)
        {
            Debug.Log($"Object {i++}: {o} InUse: {o.InUse}");
        }
    }

    public void SetArea(List<Tile> tiles)
    {
        if (tiles == null) return;
        _tiles = tiles;
        ResetArea();
        
        if (DrawArea) GenerateAreaMesh(_tiles);
        
        if (!DrawOutline) return;
        
        //HideArea();
        foreach (Tile tile in _tiles)
        {
            foreach (var (dir, neighbour) in tile.GetOrthAdjDict())
            {
                if (!_strategy.ShouldPlaceOnEdge(neighbour)) continue;
                
                OutlineObject obj = GetObject();
                obj.Object.transform.SetPositionAndRotation(tile.transform.position + dir * HOR_OFFSET + Vector3.up * VER_OFFSET,
                    Quaternion.Euler(0, 90 * (dir.z != 0 ? 1 : 0), 0)); // Rotate by 90 if z is not 0
            }
        }
    }

    void ResetArea()
    {
        if (DrawArea) _area.Clear();

        if (DrawOutline)
        {
            foreach (OutlineObject obj in _objects)
            {
                obj.Disable();
            }
        }
    }
    
    public void ShowArea()
    {
        if (DrawArea) _renderer.enabled = true;

        if (DrawOutline)
        {
            foreach (OutlineObject obj in _objects)
            {
                if (obj.InUse) obj.Show();
            }
        }
    }

    public void HideArea()
    {
        if (DrawArea) _renderer.enabled = false;

        if (DrawOutline)
        {
            foreach (OutlineObject obj in _objects)
            {
                obj.Hide();
            }
        }
    }
    
    void GenerateAreaMesh(List<Tile> tiles)
    {
        _renderer.enabled = true;

        int numberOfQuads = tiles.Count;

        _vertexBuffer = new List<Vector3>(numberOfQuads * 4);
        _triBuffer = new List<int>(numberOfQuads * 6);
        _uvs = new List<Vector2>(numberOfQuads * 4);

        foreach (Tile tile in tiles)
        {
            var pos = tile.transform.position;

            Vector3 v0 = new Vector3(pos.x - HOR_OFFSET, pos.y + BASE_VER_OFFSET, pos.z - HOR_OFFSET);
            Vector3 v1 = new Vector3(pos.x - HOR_OFFSET, pos.y + BASE_VER_OFFSET, pos.z + HOR_OFFSET);
            Vector3 v2 = new Vector3(pos.x + HOR_OFFSET, pos.y + BASE_VER_OFFSET, pos.z - HOR_OFFSET);
            Vector3 v3 = new Vector3(pos.x + HOR_OFFSET, pos.y + BASE_VER_OFFSET, pos.z + HOR_OFFSET);

            _vertexBuffer.Add(v0);
            _vertexBuffer.Add(v1);
            _vertexBuffer.Add(v2);
            _vertexBuffer.Add(v3);
            
            _uvs.Add(new Vector2(v0.x, v0.z));
            _uvs.Add(new Vector2(v1.x, v1.z));
            _uvs.Add(new Vector2(v2.x, v2.z));
            _uvs.Add(new Vector2(v3.x, v3.z));
        }

        for (int i = 0; i < numberOfQuads; i++)
        {
            _triBuffer.Add(i * 4 + 0);
            _triBuffer.Add(i * 4 + 1);
            _triBuffer.Add(i * 4 + 2);
            _triBuffer.Add(i * 4 + 1);
            _triBuffer.Add(i * 4 + 3);
            _triBuffer.Add(i * 4 + 2);
        }

        _area.Clear();
        _area.vertices = _vertexBuffer.ToArray();
        _area.uv = _uvs.ToArray();
        _area.SetTriangles(_triBuffer, 0);
        _area.RecalculateNormals();
    }
}


class OutlineObject
{
    public GameObject Object;
    public bool InUse;

    public OutlineObject(GameObject obj)
    {
        Object = obj;
        InUse = false;
    }

    public void Enable()
    {
        Object.SetActive(true);
        InUse = true;
    }

    public void Disable()
    {
        Object.SetActive(false);
        InUse = false;
    }

    public void Show()
    {
        Object.SetActive(true);
    }

    public void Hide()
    {
        Object.SetActive(false);
    }
}

[CustomEditor(typeof(TileOutliner))]
public class OutlinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (TileOutliner)target;
        script.DrawOutline = EditorGUILayout.Toggle("Draw Outline", script.DrawOutline);
        
        if (script.DrawOutline)
        {
            script.Prefab =
                (GameObject)EditorGUILayout.ObjectField("Outline Prefab", script.Prefab, typeof(GameObject), true);
        }
        
        script.DrawArea = EditorGUILayout.Toggle("Draw Area", script.DrawArea);

        if (script.DrawArea)
        {
            script.BaseMaterial =
                (Material)EditorGUILayout.ObjectField("Area Material", script.BaseMaterial, typeof(Material), true);
        }
    }
}
