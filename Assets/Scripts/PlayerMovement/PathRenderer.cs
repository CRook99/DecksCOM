using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PathRenderer : MonoBehaviour, IPlayerMovement
{
    LineRenderer _renderer;
    Vector3[] _points;
    [Tooltip("Distance between second-last and last position for line to stretch")]
    [SerializeField] float _extent;

    void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        MovementSelection.OnBeginMove += Disable;
        GridMovement.OnEndMove += Enable;
        TeamSwitcher.OnSwitch += Reset;
    }

    void OnDisable()
    {
        MovementSelection.OnBeginMove -= Disable;
        GridMovement.OnEndMove -= Enable;
        TeamSwitcher.OnSwitch -= Reset;
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        if (!TileSelection.Instance.MouseOnTile() || 
            TeamManager.Instance.Current == null ||
            !TeamManager.Instance.Current.CanMove) return;

        Tile destination = TileSelection.Instance.Current.GetComponent<Tile>();
        _points = PathfindingUtil.GetPathToTile(destination)
            .Select(x => x.gameObject.transform.position + Vector3.up * TileOutliner.VER_OFFSET)
            .ToArray();

        if (_points.Length > 2)
        {
            _points[^1].x = Mathf.Lerp(_points[^2].x, _points[^1].x, _extent);
            _points[^1].z = Mathf.Lerp(_points[^2].z, _points[^1].z, _extent);
        }
        
        _renderer.positionCount = _points.Length;
        _renderer.SetPositions(_points);
    }

    public void Enable()
    {
        _renderer.enabled = true;
    }

    public void Disable()
    {
        _renderer.positionCount = 0;
        _renderer.enabled = false;
    }

    void Reset(int _)
    {
        _renderer.positionCount = 0;
    }
}
