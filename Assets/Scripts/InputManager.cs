using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] CameraSystem cameraSystem; // Could be singleton

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.instance.MouseOnTile())
        {
            TeamManager.Instance.Current.GetComponent<PlayerMovement>().MoveToDestination(TileSelection.instance.current.GetComponent<Tile>());
        }

        // DEBUG
        if (Input.GetKeyDown(KeyCode.U)) TeamManager.Instance.Current.GetComponent<PlayerMovement>().CalculateSelectableTiles();

        // TEAM SWITCH - better way to do this
        if (Input.GetKeyDown("1") && !TeamManager.Instance.GetPlayerByIndex(0).IsDead())
        {
            cameraSystem.MoveToCharacter(TeamManager.Instance.GetPlayerByIndex(0));
        }
        else if (Input.GetKeyDown("2") && !TeamManager.Instance.GetPlayerByIndex(1).IsDead())
        {
            cameraSystem.MoveToCharacter(TeamManager.Instance.GetPlayerByIndex(1));
        }
    }
}
