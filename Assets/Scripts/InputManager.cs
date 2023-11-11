using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject currentPlayer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.instance.MouseOnTile())
        {
            currentPlayer.GetComponent<PlayerMovement>().MoveToDestination(TileSelection.instance.current.GetComponent<Tile>());
        }

        if (Input.GetKeyDown(KeyCode.U)) currentPlayer.GetComponent<PlayerMovement>().CalculateSelectableTiles();
    }
}
