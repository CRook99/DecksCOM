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
    //List<TargetLineHandler> _lines = Enumerable.Repeat(new TargetLineHandler(), 5).ToList();
    TargetLineHandler _targetLineHandler;

    int _numTargetsToSelect;
    int _range;
    bool _splash;
    int _splashRadius;

    bool _active;
    
    public static event Action OnEnterTargeting;
    public static event Action OnExitTargeting;
    public static event Action OnTargetSwitch;

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
            Debug.Log("Fire");
            ExitTargeting();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Tab)) CycleForward();
        if (Input.GetKeyDown(KeyCode.LeftShift)) CycleBackward();

        if (Input.GetKeyDown("y"))
        {
            _selections.Add(CurrentTarget);
            _targetLineHandler.AddCurrentLine();
        }
        // MAY MOVE TO OWN CLASS
    }

    void CycleAdditionals()
    {
        _targetLineHandler.UpdateCurrentLine(CurrentPlayer.Center, CurrentTarget.Center);
        if (_splash) UpdateSplash();
    }

    void CycleForward()
    {
        if (_targets.Count < 2) return;

        int index = _targets.IndexOf(CurrentTarget) + 1;
        CurrentTarget = _targets[index >= _targets.Count ? 0 : index];
        
        CycleAdditionals();
        
        OnTargetSwitch?.Invoke();
    }
    
    void CycleBackward()
    {
        if (_targets.Count < 2) return;

        int index = _targets.IndexOf(CurrentTarget) - 1;
        CurrentTarget = _targets[index < 0 ? _targets.Count - 1 : index];
        
        CycleAdditionals();
        
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
        
        GenerateRange(CurrentPlayer.GetCurrentTile(), data.Range);

        if (_targets.Count == 0)
        {
            ExitTargeting();
        }

        _numTargetsToSelect = data.Targets;
        _range = data.Range;
        _splash = data.Splash;
        _splashRadius = data.SplashRadius;
        UpdateSplash();
        _targetLineHandler.UpdateCurrentLine(CurrentPlayer.Center, CurrentTarget.Center);

        _active = true;
        OnEnterTargeting?.Invoke();
        OnTargetSwitch?.Invoke();
        CurrentPlayer.SetInactive();
    }
    
    public void ExitTargeting()
    {
        _active = false;
        _targets.Clear();
        _selections.Clear();
        RangeOutliner.HideArea();

        foreach (TileOutliner o in _splashAreas)
        {
            o.HideArea();
        }
        
        _targetLineHandler.Clear();
        
        OnExitTargeting?.Invoke();
        CurrentPlayer.SetActive();
    }

    
    public void GenerateRange(Tile origin, int range)
    {
        _targets.Clear();
        CurrentTarget = null;
        
        List<Tile> targetableTiles = PathfindingUtil.FindTargetableTiles(origin, range);
        RangeOutliner.SetArea(targetableTiles);

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
