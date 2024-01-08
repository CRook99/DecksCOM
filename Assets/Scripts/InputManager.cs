using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.instance.MouseOnTile() && TeamManager.Instance.Current.CanMove)
        {
            TeamManager.Instance.Current.Move(TileSelection.instance.current.GetComponent<Tile>());
        }

        // DEBUG
        if (Input.GetKeyDown(KeyCode.U)) TeamManager.Instance.Current.GetComponent<PlayerMovement>().CalculateSelectableTiles();

        // TEAM SWITCH - better way to do this
        
    }
}
