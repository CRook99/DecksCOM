using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public void FindEnemiesInRange(Tile origin, int range)
    {
        List<Tile> targetableTiles = PathfindingUtil.FindTargetableTiles(origin, range);
        
    }
}
