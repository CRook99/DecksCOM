using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileOutliner : MonoBehaviour
{
    public static float HOR_OFFSET = 0.5f;
    public static float VER_OFFSET = 0.55f;
    
    public GameObject Prefab;
    int _poolSize = 200;
    List<GameObject> _objects = new();
    
    public Material BaseMaterial;
    Mesh _moveArea;
    MeshFilter _filter;
    MeshRenderer _renderer;
    List<Vector3> _vertexBuffer;
    List<int> _triBuffer;
    List<int> _subBuffer;

    IOutlineDecisionStrategy _strategy;

    protected virtual void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateObject();
        }

        _moveArea = new Mesh();
        _filter = GetComponent<MeshFilter>();
        _filter.mesh = _moveArea;
        _renderer = GetComponent<MeshRenderer>();
        _renderer.sharedMaterial = BaseMaterial;
    }

    public void SetDecisionStrategy(IOutlineDecisionStrategy strategy)
    {
        if (strategy == null) return;
        
        _strategy = strategy;
    }
    
    GameObject CreateObject()
    {
        GameObject obj = Instantiate(Prefab, Vector3.zero, Quaternion.identity, transform);
        obj.SetActive(false);
        _objects.Add(obj);
        return obj;
    }
    
    GameObject GetObject()
    {
        GameObject obj = _objects.Find(b => !b.activeInHierarchy);
        if (obj == null)
        {
            obj = CreateObject();
        }
        obj.SetActive(true);
        return obj;
    }
    
    public void ShowOutline(List<Tile> tiles)
    {
        HideOutline();
        
        foreach (Tile tile in tiles)
        {
            foreach (var (dir, neighbour) in tile.GetOrthAdjDict())
            {
                if (!_strategy.ShouldPlaceOnEdge(neighbour)) continue;
                
                GameObject obj = GetObject();
                obj.transform.SetPositionAndRotation(tile.transform.position + dir * HOR_OFFSET + Vector3.up * VER_OFFSET,
                    Quaternion.Euler(0, 90 * (dir.z != 0 ? 1 : 0), 0)); // Rotate by 90 if z is not 0
            }
        }
        
        GenerateMoveAreaMesh(tiles);
    }

    public void HideOutline()
    {
        foreach (GameObject obj in _objects)
        {
            obj.SetActive(false);
        }
    }
    
    void GenerateMoveAreaMesh(List<Tile> tiles)
    {
        _renderer.enabled = true;

        int numberOfQuads = tiles.Count;

        _vertexBuffer = new List<Vector3>(numberOfQuads * 4);
        _triBuffer = new List<int>(numberOfQuads * 6);

        foreach (Tile tile in tiles)
        {
            var pos = tile.transform.position;

            _vertexBuffer.Add(new Vector3(pos.x - HOR_OFFSET, pos.y + VER_OFFSET, pos.z - HOR_OFFSET));
            _vertexBuffer.Add(new Vector3(pos.x - HOR_OFFSET, pos.y + VER_OFFSET, pos.z + HOR_OFFSET));
            _vertexBuffer.Add(new Vector3(pos.x + HOR_OFFSET, pos.y + VER_OFFSET, pos.z - HOR_OFFSET));
            _vertexBuffer.Add(new Vector3(pos.x + HOR_OFFSET, pos.y + VER_OFFSET, pos.z + HOR_OFFSET));
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

        _moveArea.Clear();
        _moveArea.vertices = _vertexBuffer.ToArray();
        _moveArea.SetTriangles(_triBuffer, 0);
        _moveArea.RecalculateNormals();
    }
}