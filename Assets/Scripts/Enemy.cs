using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public GameObject HealthBarPrefab;
    public Transform HealthBarAnchor;
    EnemyHealthBarUI _healthBar;
    
    public event Action OnTarget;
    public event Action OnUntarget;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        EnemyManager.Instance.AddEnemy(this);
        Transform canvas = GameObject.FindGameObjectWithTag("EnemyUI").transform;
        EnemyHealthBarUI bar = Instantiate(HealthBarPrefab, canvas).GetComponent<EnemyHealthBarUI>();
        bar.RegisterEnemy(this, _healthManager);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown("9")) Heal(10);
        if (Input.GetKeyDown("0")) Damage(10);
        
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
    }

    public override void Damage(int amount)
    {
        base.Damage(amount);
    }

    public void Target()
    {
        OnTarget?.Invoke();
    }
    
    public void Untarget()
    {
        OnUntarget?.Invoke();
    }
}
