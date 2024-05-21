using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSelection : MonoBehaviour
{
    public static event Action OnBeginMove;
    Tile destination;

    void Awake()
    {
        TargetingSystem.OnEnterTargeting += Disable;
        TargetingSystem.OnExitTargeting += Enable;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && TileSelection.Instance.MouseOnTile() && TeamManager.Instance.Current.CanMove)
        {
            destination = TileSelection.Instance.Current.GetComponent<Tile>();
            BeginMove();
        }
    }

    void Enable()
    {
        enabled = true;
    }

    void Disable()
    {
        enabled = false;
    }

    void BeginMove()
    {
        Player player = TeamManager.Instance.Current;
        OnBeginMove?.Invoke();
        player.Move(destination);
    }
}
