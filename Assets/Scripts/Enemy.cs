using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthManager))]
public class Enemy : MonoBehaviour
{
    GridMovement _gridMovement;
    HealthManager _healthManager;

    public GameObject HealthBarPrefab;
    public Transform HealthBarAnchor;
    EnemyHealthBarUI _healthBar;
    
    public event Action OnHeal;
    public event Action OnDamage;
    public event Action OnTarget;
    public event Action OnUntarget;
    
    public Vector3 Center => transform.position + Vector3.up * 0.5f;
    
    public bool Dead { get; private set; }
    public bool CanMove { get; private set; }

    void Awake()
    {
        _gridMovement = GetComponent<GridMovement>();
        _healthManager = GetComponent<HealthManager>();
    }

    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
        Transform canvas = GameObject.FindGameObjectWithTag("EnemyUI").transform;
        EnemyHealthBarUI bar = Instantiate(HealthBarPrefab, canvas).GetComponent<EnemyHealthBarUI>();
        bar.RegisterEnemy(this, _healthManager);
    }

    void Update()
    {
        if (Input.GetKeyDown("9")) Heal(10);
        if (Input.GetKeyDown("0")) Damage(10);
        
    }

    public void Heal(int amount)
    {
        _healthManager.Heal(amount);
        OnHeal?.Invoke();
    }

    public void Damage(int amount)
    {
        _healthManager.TakeDamage(amount);
        OnDamage?.Invoke();
    }

    public void Target()
    {
        OnTarget?.Invoke();
    }
    
    public void Untarget()
    {
        OnUntarget?.Invoke();
    }
    
    public Tile GetCurrentTile()
    {
        return _gridMovement.GetCurrentTile();
    }
}
