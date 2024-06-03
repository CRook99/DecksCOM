using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyHealthBarUI : MonoBehaviour
{
    Camera _mainCamera;
    Transform _anchor;
    bool _maximised;

    [SerializeField] Image _fill;
    [SerializeField] TextMeshProUGUI _amount;
    [SerializeField] List<GameObject> _maximiseElements;
    [SerializeField] TextMeshProUGUI _coverPercent;
    
    RectTransform _rect;
    
    Enemy _enemy;
    HealthManager _health;

    void Awake()
    {
        _mainCamera = Camera.main;
        _rect = GetComponent<RectTransform>();
        
        Minimize();
    }

    void Update()
    {
        transform.position = _mainCamera.WorldToScreenPoint(_anchor.position) + Vector3.up * (_maximised ? 30f : 10f);
        if (Input.GetKeyDown("7")) Maximize();
        if (Input.GetKeyDown("8")) Minimize();
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
        e.OnDamage += Minimize; // TODO Will minimize on fire
        e.OnTarget += Maximize;
        e.OnUntarget += Minimize;
    }

    void UpdateValues()
    {
        _fill.fillAmount = _health.NormalizedHealth();
        _amount.text = _health.Health.ToString();
    }

    void Maximize()
    {
        foreach (GameObject element in _maximiseElements)
        {
            element.SetActive(true);
        }
        
        _maximised = true;
        
        transform.DOScale(2f, 0.1f).SetEase(Ease.OutCubic);
    }

    void Minimize()
    {
        foreach (GameObject element in _maximiseElements)
        {
            element.SetActive(false);
        }

        _maximised = false;
        
        transform.DOScale(1f, 0.1f).SetEase(Ease.OutCubic);
    }
}
