using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public static TargetingSystem Instance { get; private set; }
    [SerializeField] List<Enemy> _targets;
    public Player CurrentPlayer;
    public Enemy CurrentTarget;
    TileOutliner _rangeOutline;
    [SerializeField] TileOutliner _splashOutline;

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
        
        _rangeOutline = GetComponent<TileOutliner>();
        _rangeOutline.SetDecisionStrategy(new TargetingStrategy());
        _splashOutline.SetDecisionStrategy(new TargetingStrategy());
    }

    void Update()
    {
        // if (Input.GetKeyDown("k")) GenerateRange(TeamManager.Instance.Current.GetCurrentTile(), 5);
        // if (Input.GetKeyDown("l")) DeactivateTargeting();
        
        if (Input.GetKeyDown(KeyCode.Tab)) CycleForward();
        if (Input.GetKeyDown(KeyCode.LeftShift)) CycleBackward();
    }

    void CycleForward()
    {
        if (_targets.Count < 2) return;

        int index = _targets.IndexOf(CurrentTarget) + 1;
        CurrentTarget = _targets[index >= _targets.Count ? 0 : index];
        OnTargetSwitch?.Invoke();
    }
    
    void CycleBackward()
    {
        if (_targets.Count < 2) return;

        int index = _targets.IndexOf(CurrentTarget) - 1;
        CurrentTarget = _targets[index < 0 ? _targets.Count - 1 : index];
        OnTargetSwitch?.Invoke();
    }

    public void EnterTargeting(WeaponData data)
    {
        CurrentPlayer = TeamManager.Instance.Current;
        
        GenerateRange(CurrentPlayer.GetCurrentTile(), data.Range);

        if (_targets.Count == 0)
        {
            ExitTargeting();
        }
        
        OnEnterTargeting?.Invoke();
        OnTargetSwitch?.Invoke();
        TeamManager.Instance.Current.SetInactive();
    }
    
    public void ExitTargeting()
    {
        _targets.Clear();
        _rangeOutline.HideArea();
        OnExitTargeting?.Invoke();
        TeamManager.Instance.Current.SetActive();
    }

    public void GenerateRange(Tile origin, int range)
    {
        _targets.Clear();
        CurrentTarget = null;
        
        List<Tile> targetableTiles = PathfindingUtil.FindTargetableTiles(origin, range);
        _rangeOutline.SetArea(targetableTiles);

        float min = Mathf.Infinity;
        foreach (Enemy e in EnemyManager.Instance.GetEnemies())
        {
            float distance = Vector3.Distance(e.transform.position, TeamManager.Instance.Current.transform.position);

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
