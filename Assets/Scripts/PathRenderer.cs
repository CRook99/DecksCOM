using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    LineRenderer _renderer;
    Vector3[] _points;

    void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        MovementSelection.OnBeginMove += Hide;
        GridMovement.OnEndMove += Show;
        TeamSwitcher.OnSwitch += Reset;
    }

    void OnDisable()
    {
        MovementSelection.OnBeginMove -= Hide;
        GridMovement.OnEndMove -= Show;
        TeamSwitcher.OnSwitch -= Reset;
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        if (!TileSelection.Instance.MouseOnTile() || !TeamManager.Instance.Current.CanMove) return;

        Tile destination = TileSelection.Instance.Current.GetComponent<Tile>();
        _points = PathfindingUtil.GetPathToTile(destination)
            .Select(x => x.gameObject.transform.position + Vector3.up * TileOutline.VER_OFFSET)
            .ToArray();

        if (_points.Length > 2)
        {
            _points[^1].x = Median(_points[^1].x, _points[^2].x);
            _points[^1].z = Median(_points[^1].z, _points[^2].z);
        }
        
        _renderer.positionCount = _points.Length;
        _renderer.SetPositions(_points);
    }

    float Median(float a, float b)
    {
        return (a + b) / 2;
    }

    void Show()
    {
        _renderer.enabled = true;
    }

    void Hide(GameObject _)
    {
        _renderer.positionCount = 0;
        _renderer.enabled = false;
    }

    void Reset(int _)
    {
        _renderer.positionCount = 0;
    }
}
