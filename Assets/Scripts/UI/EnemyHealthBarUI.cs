using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyHealthBarUI : MonoBehaviour
{
    Camera _mainCamera;
    Transform _anchor;

    [SerializeField] Image _fill;
    
    Enemy _enemy;
    HealthManager _health;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        transform.position = _mainCamera.WorldToScreenPoint(_anchor.position) + Vector3.up * 10f;
    }

    public void RegisterEnemy(Enemy e, HealthManager h)
    {
        if (e == null)
            throw new ArgumentNullException();
        _enemy = e;
        _health = h;
        _anchor = e.HealthBarAnchor;

        e.OnHeal += UpdateValues;
        e.OnDamage += UpdateValues;
    }

    void UpdateValues()
    {
        _fill.fillAmount = _health.NormalizedHealth();
    }
}
