using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private static TileSelection _instance;
    public static TileSelection Instance { get { return _instance; } }
    public GameObject current;
    public GameObject previous;
    private LayerMask TileMask;

    void Awake()
    {
        _instance = this;
        current = gameObject;
        previous = gameObject;
        TileMask = LayerMask.GetMask("Tile");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, TileMask))
        {
            current = hit.collider.gameObject;

        }

        if (current != previous)
        {
            current.GetComponent<Tile>().ShowShields();

            if (previous.tag == "Tile")
            {
                previous.GetComponent<Tile>().HideShields();
            }
            
            previous = current;
        }
    }

    public bool MouseOnTile()
    {
        return current.tag == "Tile";
    }
}
