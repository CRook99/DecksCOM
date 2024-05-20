using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public static TargetingSystem Instance { get; private set; }
    [SerializeField] List<Enemy> _targets;
    public Enemy CurrentTarget;
    TileOutliner _outliner;
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
        
        _outliner = GetComponent<TileOutliner>();
        _outliner.SetDecisionStrategy(new TargetingStrategy());
    }

    void Update()
    {
        if (Input.GetKeyDown("k")) ActivateTargeting(TeamManager.Instance.Current.GetCurrentTile(), 5);
        if (Input.GetKeyDown("l")) DeactivateTargeting();
    }

    public void ActivateTargeting(Tile origin, int range)
    {
        _targets.Clear();
        CurrentTarget = null;
        
        List<Tile> targetableTiles = PathfindingUtil.FindTargetableTiles(origin, range);
        _outliner.SetArea(targetableTiles);

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

        if (CurrentTarget == null) 
        {
            DeactivateTargeting();
            return; // TODO Add 'No targets!' logic
        }
        
        // There is at least one enemy in range
        _active = true;

        OnEnterTargeting?.Invoke();
        OnTargetSwitch?.Invoke();
        TeamManager.Instance.Current.SetInactive();
    }

    public void DeactivateTargeting()
    {
        _active = false;
        _targets.Clear();
        _outliner.HideArea();
        OnExitTargeting?.Invoke();
        TeamManager.Instance.Current.SetActive();
    }
}
