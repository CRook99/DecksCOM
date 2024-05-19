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
    }

    void OnDisable()
    {
        GameState.OnBeginEnemyTurn -= HideOutline;
    }

    public void ShowOutline(List<Tile> tiles)
    {
        _outliner.ShowOutline(tiles);
    }

    public void HideOutline()
    {
        if (_outliner == null)
        {
            Debug.LogWarning("MovementOutline._outliner is null");
            return;
        }
        _outliner.HideOutline();
    }

    
}
