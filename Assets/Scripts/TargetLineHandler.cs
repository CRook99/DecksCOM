using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TargetLineHandler : MonoBehaviour
{
    LineRenderer _renderer;
    int _registeredTargets;

    void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    public void UpdateCurrentLine(Vector3 origin, Vector3 destination)
    {
        LOSInfo info = LineOfSightUtil.GetLineOfSight(origin, destination);
        
        _renderer.positionCount = _registeredTargets * 2 + 2;
        _renderer.SetPosition(_renderer.positionCount - 2, origin);
        _renderer.SetPosition(_renderer.positionCount - 1, info.ImpactPoint);
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
