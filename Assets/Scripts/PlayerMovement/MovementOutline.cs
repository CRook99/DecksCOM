using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TileOutliner))]
public class MovementOutline : MonoBehaviour
{
    public static MovementOutline Instance { get; private set; }

    TileOutliner _outliner;
    
    void Awake()
    {
        Instance = this;
        _outliner = GetComponent<TileOutliner>();
        _outliner.SetDecisionStrategy(new MovementStrategy());
    }

    void OnEnable()
    {
        GameState.OnBeginEnemyTurn += HideOutline;
        TargetingSystem.OnEnterTargeting += HideOutline;
        TargetingSystem.OnExitTargeting += ShowOutline;
    }

    void OnDisable()
    {
        GameState.OnBeginEnemyTurn -= HideOutline;
        TargetingSystem.OnEnterTargeting -= HideOutline;
        TargetingSystem.OnExitTargeting -= ShowOutline;
    }

    public void SetArea(List<Tile> tiles)
    {
        _outliner.SetArea(tiles);
        ShowOutline();
    }

    public void ShowOutline()
    {
        if (!TeamManager.Instance.Current.CanMove) return;
        
        _outliner.ShowArea();
    }

    public void HideOutline()
    {
        if (_outliner == null)
        {
            Debug.LogWarning("MovementOutline._outliner is null");
            return;
        }
        _outliner.HideArea();
    }

    
}
