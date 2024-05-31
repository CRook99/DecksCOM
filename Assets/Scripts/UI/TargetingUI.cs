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
    }

    void OnEnable()
    {
        TargetingSystem.Instance.OnEnterTargeting += OnEnterTargetingHandler;
        TargetingSystem.Instance.OnExitTargeting += OnExitTargetingHandler;
        TargetingSystem.Instance.OnTargetSwitch += OnSwitchHandler;
    }

    void OnDisable()
    {
        TargetingSystem.Instance.OnEnterTargeting -= OnEnterTargetingHandler;
        TargetingSystem.Instance.OnExitTargeting -= OnExitTargetingHandler;
        TargetingSystem.Instance.OnTargetSwitch -= OnSwitchHandler;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 screenPos = main.WorldToScreenPoint(TargetingSystem.Instance.CurrentTarget.Center);
        _indicator.transform.position = screenPos;
    }

    void OnEnterTargetingHandler()
    {
        gameObject.SetActive(true);
    }

    void OnExitTargetingHandler()
    {
        gameObject.SetActive(false);
    }

    void OnSwitchHandler()
    {
        IEnumerator WaitToShrink()
        {
            yield return null;
            _indicator.ShrinkAnim();
        }

        StartCoroutine(WaitToShrink()); // IMPROVE make it not flash in middle

    }
}
