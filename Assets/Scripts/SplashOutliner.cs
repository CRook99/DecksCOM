using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashOutliner
{
    TileOutliner _outliner;
    List<Tile> _area;
    public int Radius;

    public void SetAreaFromTile(Tile tile)
    {
        _outliner.SetArea(PathfindingUtil.FindTargetableTiles(tile, Radius));
    }

    public void SetAreaFromList(List<Tile> tiles)
    {
        _outliner.SetArea(tiles);
    }

    public void ShowArea()
    {
        _outliner.ShowArea();
    }

    public void HideArea()
    {
        _outliner.HideArea();
    }
}
