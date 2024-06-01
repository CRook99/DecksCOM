using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSelection : MonoBehaviour, IPlayerMovement
{
    public static event Action OnBeginMove;
    Tile destination;

    void Update()
    {
        if (!TeamManager.Instance.Current.CanMove) return;
        
        if (Input.GetMouseButtonDown(1) && TileSelection.Instance.MouseOnTile() && TeamManager.Instance.Current.CanMove)
        {
            destination = TileSelection.Instance.Current.GetComponent<Tile>();
            BeginMove();
        }
    }
    
    void BeginMove()
    {
        Player player = TeamManager.Instance.Current;
        OnBeginMove?.Invoke();
        player.Move(destination);
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }

    
}
