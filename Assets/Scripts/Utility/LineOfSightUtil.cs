using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightUtil
{
    static RaycastHit[] _hits = new RaycastHit[10];
    static LayerMask _defaultMask = LayerMask.GetMask("Tile", "Env_Dynamic", "Env_Static", "Cover");
    
    public static LOSInfo GetLineOfSight(Vector3 origin, Vector3 target)
    {
        return GetLineOfSight(origin, target, _defaultMask);
    }
    
    public static LOSInfo GetLineOfSight(Vector3 origin, Vector3 target, LayerMask mask)
    {
        Vector3 direction = target - origin;
        float distance = direction.magnitude;
        direction.Normalize();
        
        int hitCount = Physics.RaycastNonAlloc(origin, direction, _hits, distance);

        for (int i = 0; i < hitCount; i++)
        {
            var hit = _hits[i];

            if (hit.collider != null && (mask.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                return new LOSInfo(hit.point, false);
            }
        }

        return new LOSInfo(target, true);
    }
}

public struct LOSInfo
{
    public Vector3 ImpactPoint;
    public bool ClearLOS;

    public LOSInfo(Vector3 point, bool clear) 
        => (ImpactPoint, ClearLOS) = (point, clear);
}