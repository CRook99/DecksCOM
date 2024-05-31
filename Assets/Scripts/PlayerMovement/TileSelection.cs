using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    public static TileSelection Instance { get; private set; }
    public GameObject Current { get; private set; }
    public GameObject Previous { get; private set; }
    LayerMask _tileMask;

    Camera main;

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
        
        Current = gameObject;
        Previous = gameObject;
        _tileMask = LayerMask.GetMask("Tile");
        main = Camera.main;

        TargetingSystem.Instance.OnEnterTargeting += HideCurrentShields;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileMask))
        {
            Current = hit.collider.gameObject;

        }

        if (Current != Previous)
        {
            Current.GetComponent<Tile>().ShowShields();

            if (Previous.CompareTag("Tile"))
            {
                Previous.GetComponent<Tile>().HideShields();
            }
            
            Previous = Current;
        }
    }

    public bool MouseOnTile()
    {
        return Current.CompareTag("Tile");
    }

    void HideCurrentShields()
    {
        Current.GetComponent<Tile>().HideShields();
    }
}
