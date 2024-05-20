using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileOutliner))]
public class TargetingOutline : MonoBehaviour
{
    //public static TargetingOutline Instance { get; private set; }
    
    TileOutliner _outliner;
    
    void Awake()
    {
        //Instance = this;
        _outliner = GetComponent<TileOutliner>();
        _outliner.SetDecisionStrategy(new TargetingStrategy());
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            ShowOutline(TeamManager.Instance.Current.GetCurrentTile(), 12);
        }
    }

    public void ShowOutline(Tile origin, int range)
    {
        _outliner.ShowArea(PathfindingUtil.FindTargetableTiles(origin, range));
    }

    public void HideOutline()
    {
        _outliner.HideArea();
    }
}
