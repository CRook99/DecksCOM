using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOutliner : MonoBehaviour
{
    public static float HOR_OFFSET = 0.5f;
    public static float VER_OFFSET = 0.55f;
    
    public GameObject Prefab;
    int _poolSize = 200;
    List<GameObject> _objects = new();

    IOutlineDecisionStrategy _strategy;

    protected virtual void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateObject();
        }
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
    }

    public void HideOutline()
    {
        foreach (GameObject obj in _objects)
        {
            obj.SetActive(false);
        }
    }
}