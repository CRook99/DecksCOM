using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSelection : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.Instance.MouseOnTile() && TeamManager.Instance.Current.CanMove)
        {
            StartCoroutine(BeginMove());
        }
    }

    IEnumerator BeginMove()
    {
        TeamSwitcher.Instance.Disable(); // Re-enabling in MoveToDestination#GridMovement.cs
        StartCoroutine(CameraSystem.Instance.FollowCharacterMovement(TeamManager.Instance.Current));
        Tile destination = TileSelection.Instance.current.GetComponent<Tile>();
        yield return new WaitForSeconds(CameraSystem.MOVE_DURATION);
        TeamManager.Instance.Current.Move(destination);
        yield break;
    }
}
