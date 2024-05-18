using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSelection : MonoBehaviour
{
    // IMPROVE - Make this not have GameObject (only use is CameraSystem)
    public static event Action OnBeginMove;
    Tile destination;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.Instance.MouseOnTile() && TeamManager.Instance.Current.CanMove)
        {
            destination = TileSelection.Instance.Current.GetComponent<Tile>();
            BeginMove();
        }
    }

    void BeginMove()
    {
        Player player = TeamManager.Instance.Current;
        //OnBeginMove?.Invoke(player.gameObject);
        OnBeginMove?.Invoke();
        player.Move(destination);
    }
}
