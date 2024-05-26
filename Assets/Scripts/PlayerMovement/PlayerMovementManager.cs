using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager Instance { get; private set; }
    public List<IPlayerMovement> _components;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }

        _components = new List<IPlayerMovement>();

        TargetingSystem.OnEnterTargeting += Disable;
        TargetingSystem.OnExitTargeting += Enable;
        GameState.OnBeginEnemyTurn += Disable;
        GameState.OnBeginPlayerTurn += Enable;
        Card.StaticBeginDragEvent += Disable;
    }

    public void Enable()
    {
        foreach (var c in _components)
        {
            c.Enable();
        }
    }
    
    public void Disable()
    {
        foreach (var c in _components)
        {
            c.Disable();
        }
    }
    

    public void RegisterComponent(IPlayerMovement c)
    {
        if (c == null || _components.Contains(c)) return;
        _components.Add(c);
    }
    
    public void DeregisterComponent(IPlayerMovement c)
    {
        if (c == null || _components.Contains(c)) return;
        _components.Remove(c);
    }
}
