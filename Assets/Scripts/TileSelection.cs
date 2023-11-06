using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    public static TileSelection instance;
    public GameObject current;
    public GameObject nullTile;

    void Awake()
    {
        instance = this;
        current = gameObject; // Ensures not null on start
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            current = hit.collider.gameObject;
        }
    }

    public bool MouseOnTile()
    {
        return current.tag == "Tile";
    }
}
