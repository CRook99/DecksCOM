using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSwitcher : MonoBehaviour
{
    private static TeamSwitcher _instance;
    public static TeamSwitcher Instance { get { return _instance; } }
    bool canSwitch = true;

    public static event Action OnSwitch;

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (!canSwitch) return;

        if (Input.GetKeyDown("1") && !TeamManager.Instance.GetPlayerByIndex(0).Dead)
        {
            StartCoroutine(Switch(0));
        }
        else if (Input.GetKeyDown("2") && !TeamManager.Instance.GetPlayerByIndex(1).Dead)
        {
            StartCoroutine(Switch(1));
        }
        else if (Input.GetKeyDown("3") && !TeamManager.Instance.GetPlayerByIndex(2).Dead)
        {
            StartCoroutine(Switch(2));
        }
    }

    IEnumerator Switch(int index)
    {
        TeamManager.Instance.Current.SetInactive();
        TeamManager.Instance.SetCurrent(index);
        TeamManager.Instance.Current.SetActive();
        
        OnSwitch?.Invoke();
        
        //DashButtonUI.Instance.UpdateCostForCurrentPlayer(); // IMPROVE - Move to event
        
        Disable();
        CameraSystem.Instance.MoveToObject(TeamManager.Instance.Current.gameObject);
        yield return new WaitForSeconds(CameraSystem.MOVE_DURATION);
        Enable();
    }

    public void Enable() { canSwitch = true; }
    public void Disable() { canSwitch = false; }
}
