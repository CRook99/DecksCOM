using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TargetLineHandler : MonoBehaviour
{
    LineRenderer _renderer;
    RaycastHit[] _hits = new RaycastHit[5];
    LayerMask _mask;
    int _registeredTargets;

    public bool CanHitTarget;

    void Awake()
    {
        _renderer = GetComponent<LineRenderer>(); 
        _mask = LayerMask.GetMask("Tile", "Env_Dynamic", "Env_Static", "Cover");
    }

    public void UpdateCurrentLine(Vector3 origin, Vector3 destination)
    {
        Vector3 direction = destination - origin;
        float distance = Vector3.Distance(origin, destination);
        Physics.RaycastNonAlloc(origin, direction, _hits, distance);
        
        Vector3 stopPoint = destination;
        CanHitTarget = true;
        foreach (var hit in _hits)
        {
            if (hit.collider == null) break;

            if ((_mask.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                stopPoint = hit.point;
                CanHitTarget = false;
                break;
            }
        }
        
        _renderer.positionCount = _registeredTargets * 2 + 2;
        _renderer.SetPosition(_renderer.positionCount - 2, origin);
        _renderer.SetPosition(_renderer.positionCount - 1, stopPoint);
    }

    public void AddCurrentLine()
    {
        _registeredTargets++;
    }

    public void Clear()
    {
        _renderer.positionCount = 0;
    }
}
