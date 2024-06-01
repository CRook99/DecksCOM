using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    List<IPlayerMovement> _components;

    void Awake()
    {
        _components = GetComponentsInChildren<IPlayerMovement>().ToList();

        TargetingSystem.Instance.OnEnterTargeting += Disable;
        TargetingSystem.Instance.OnExitTargeting += Enable;
        GameState.OnBeginEnemyTurn += Disable;
        GameState.OnBeginPlayerTurn += Enable;
        Card.StaticBeginDragEvent += Disable;
    }

    public void Enable()
    {
        _components.ForEach(c => c.Enable());
    }
    
    public void Disable()
    {
        _components.ForEach(c => c.Disable());
    }
}
