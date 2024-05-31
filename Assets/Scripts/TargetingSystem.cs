using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public static TargetingSystem Instance { get; private set; }
    [SerializeField] List<Enemy> _targets;
    [SerializeField] List<Enemy> _selections;
    public Player CurrentPlayer;
    public Enemy CurrentTarget;
    
    public TileOutliner RangeOutliner;
    List<TileOutliner> _splashAreas = new();
    TargetLineHandler _targetLineHandler;

    int _numTargetsToSelect;
    int _range;
    bool _splash;
    int _splashRadius;
    bool _ignoreCover;

    bool _active;
    
    public event Action OnEnterTargeting;
    public event Action OnExitTargeting;
    public event Action OnTargetSwitch;

    public float ExtraBuffer = 0.9f; // Additional distance to search for targets (will change if we go tile-based)

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        RangeOutliner.SetDecisionStrategy(new TargetingStrategy());

        foreach (Transform t in transform)
        {
            TileOutliner o = t.GetComponent<TileOutliner>();
            if (!o) continue;
            _splashAreas.Add(o);
            o.SetDecisionStrategy(new TargetingStrategy());
        }

        _targetLineHandler = GetComponent<TargetLineHandler>();
        _active = false;
    }

    void Update()
    {
        // MAY MOVE TO OWN CLASS
        if (!_active) return;
        
        if (_selections.Count == _numTargetsToSelect)
        {
            ExitTargeting();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Tab)) CycleForward();
        if (Input.GetKeyDown(KeyCode.LeftShift)) CycleBackward();

        if (Input.GetKeyDown("y")) // Replace with UI click
        {
            _selections.Add(CurrentTarget);
            if (!_ignoreCover) _targetLineHandler.AddCurrentLine();
        }
        // MAY MOVE TO OWN CLASS
    }
    

    void CycleForward()
    {
        if (_targets.Count < 2) return;
        
        CurrentTarget.Untarget();
        CurrentTarget = _targets[(_targets.IndexOf(CurrentTarget) + 1) % _targets.Count];
        
        if (!_ignoreCover) _targetLineHandler.UpdateCurrentLine(CurrentPlayer.Center, CurrentTarget.Center);
        if (_splash) UpdateSplash();
        CurrentTarget.Target();
        
        OnTargetSwitch?.Invoke();
    }
    
    void CycleBackward()
    {
        if (_targets.Count < 2) return;
        
        CurrentTarget.Untarget();
        CurrentTarget = _targets[(_targets.IndexOf(CurrentTarget) - 1 + _targets.Count) % _targets.Count];
        
        if (!_ignoreCover) _targetLineHandler.UpdateCurrentLine(CurrentPlayer.Center, CurrentTarget.Center);
        if (_splash) UpdateSplash();
        CurrentTarget.Target();
        
        OnTargetSwitch?.Invoke();
    }

    void UpdateSplash()
    {
        List<Tile> tiles = PathfindingUtil.FindTargetableTiles(CurrentTarget.GetCurrentTile(), _splashRadius);
        _splashAreas[_selections.Count].SetArea(tiles);
        _splashAreas[_selections.Count].ShowArea();
    }
    

    public void EnterTargeting(WeaponData data)
    {
        CurrentPlayer = TeamManager.Instance.Current;
        
        FindTargets(data.Range);
        if (_targets.Count == 0)
        {
            ExitTargeting();
            return;
        }
        
        // Load data from card scriptable object
        _numTargetsToSelect = data.Targets;
        _range = data.Range;
        _splash = data.SplashRadius > 0;
        _splashRadius = data.SplashRadius;
        _ignoreCover = data.IgnoreCover;
        
        // Visuals
        RangeOutliner.SetArea(PathfindingUtil.FindTargetableTiles(CurrentPlayer.GetCurrentTile(), data.Range));
        if (!_ignoreCover) _targetLineHandler.UpdateCurrentLine(CurrentPlayer.Center, CurrentTarget.Center);
        if (_splash) UpdateSplash();
        CurrentTarget.Target();
        
        // State
        _active = true;
        _selections.Clear();
        
        OnEnterTargeting?.Invoke();
        OnTargetSwitch?.Invoke();
        CurrentPlayer.SetInactive();
    }
    
    void ExitTargeting()
    {
        _active = false;
        _targets.Clear();
        _selections.Clear();
        RangeOutliner.HideArea();

        // Visuals
        foreach (TileOutliner o in _splashAreas)
        {
            o.HideArea();
        }
        
        _targetLineHandler.Clear();
        
        OnExitTargeting?.Invoke();
        CurrentPlayer.SetActive();
    }

    
    void FindTargets(int range)
    {
        // List<Tile> targetableTiles = PathfindingUtil.FindTargetableTiles(origin, range);
        // RangeOutliner.SetArea(targetableTiles);
        
        _targets.Clear();
        CurrentTarget = null;
        float min = Mathf.Infinity;
        foreach (Enemy e in EnemyManager.Instance.GetEnemies())
        {
            float distance = Vector3.Distance(e.transform.position, CurrentPlayer.transform.position);

            if (distance > range + ExtraBuffer) continue;
            
            _targets.Add(e);
            if (distance < min)
            {
                min = distance;
                CurrentTarget = e;
            }
        }
    }
}
