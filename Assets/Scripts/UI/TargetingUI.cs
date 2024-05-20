using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingUI : MonoBehaviour
{
    public TargetIndicator _indicator;
    Camera main;

    void Awake()
    {
        main = Camera.main;
        TargetingSystem.OnTargetSwitch += OnSwitchHandler;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 screenPos = main.WorldToScreenPoint(TargetingSystem.Instance.CurrentTarget.transform.position);
        _indicator.transform.position = screenPos;
    }

    void OnSwitchHandler()
    {
        _indicator.ShrinkAnim();
    }
}
