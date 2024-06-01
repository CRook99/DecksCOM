using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TileOutliner))]
public class MovementOutline : MonoBehaviour, IPlayerMovement
{
    TileOutliner _outliner;
    
    void Awake()
    {
        _outliner = GetComponent<TileOutliner>();
        _outliner.SetDecisionStrategy(new MovementStrategy());
    }

    public void SetArea(List<Tile> tiles)
    {
        _outliner.SetArea(tiles);
        Enable();
    }

    public void Enable()
    {
        if (!TeamManager.Instance.Current.CanMove) return;
        
        _outliner.ShowArea();
    }

    public void Disable()
    {
        if (_outliner == null)
        {
            Debug.LogWarning("MovementOutline._outliner is null");
            return;
        }
        _outliner.HideArea();
    }

    
}
