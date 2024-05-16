using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private static TileSelection _instance;
    public static TileSelection Instance { get { return _instance; } }
    public GameObject Current { get; private set; }
    public GameObject Previous { get; private set; }
    LayerMask _tileMask;

    void Awake()
    {
        _instance = this;
        Current = gameObject;
        Previous = gameObject;
        _tileMask = LayerMask.GetMask("Tile");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}
