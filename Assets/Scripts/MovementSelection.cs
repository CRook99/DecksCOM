using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSelection : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.instance.MouseOnTile() && TeamManager.Instance.Current.CanMove)
        {
            StartCoroutine(BeginMove());
        }
    }

    IEnumerator BeginMove()
    {
        TeamSwitcher.Instance.Disable(); // Re-enabling in MoveToDestination#GridMovement.cs
        StartCoroutine(CameraSystem.Instance.MoveToPoint(TeamManager.Instance.Current.gameObject));
        Tile destination = TileSelection.instance.current.GetComponent<Tile>();
        yield return new WaitForSeconds(CameraSystem.MOVE_DURATION + 0.2f);
        TeamManager.Instance.Current.Move(destination);
    }
}
