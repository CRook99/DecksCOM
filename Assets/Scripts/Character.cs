using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(GridMovement))]
public abstract class Character : MonoBehaviour
{
    // Components
    protected HealthManager _healthManager;
    protected GridMovement _gridMovement;

    // Events
    public event Action OnHeal;
    public event Action OnDamage;
    
    // Stats
    public bool Dead { get; protected set; }
    public bool CanMove { get; protected set; }
    
    public Vector3 Center => transform.position + Vector3.up * 0.5f;

    protected virtual void Awake()
    {
        _healthManager = GetComponent<HealthManager>();
        _gridMovement = GetComponent<GridMovement>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Heal(int amount)
    {
        _healthManager.Heal(amount);
        OnHeal?.Invoke();
    }

    public virtual void Damage(int amount)
    {
        _healthManager.TakeDamage(amount);
        OnDamage?.Invoke();
        if (_healthManager.Health == 0) Die();
    }

    public virtual void Die()
    {
        Dead = true;
    }

    public Tile GetCurrentTile()
    {
        return _gridMovement.GetCurrentTile();
    }
}
