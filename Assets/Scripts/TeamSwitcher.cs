using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSwitcher : MonoBehaviour
{
    private static TeamSwitcher _instance;
    public static TeamSwitcher Instance { get { return _instance; } }
    bool canSwitch = true;

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (!canSwitch) return;

        if (Input.GetKeyDown("1") && !TeamManager.Instance.GetPlayerByIndex(0).IsDead())
        {
            TeamManager.Instance.Current.SetInactive();
            TeamManager.Instance.SetCurrent(0);
            TeamManager.Instance.Current.SetActive();
            CameraSystem.Instance.MoveToCharacter(TeamManager.Instance.Current);
            
        }
        else if (Input.GetKeyDown("2") && !TeamManager.Instance.GetPlayerByIndex(1).IsDead())
        {
            TeamManager.Instance.Current.SetInactive();
            TeamManager.Instance.SetCurrent(1);
            TeamManager.Instance.Current.SetActive();
            CameraSystem.Instance.MoveToCharacter(TeamManager.Instance.Current);
        }
        else if (Input.GetKeyDown("3") && !TeamManager.Instance.GetPlayerByIndex(2).IsDead())
        {
            TeamManager.Instance.Current.SetInactive();
            TeamManager.Instance.SetCurrent(2);
            TeamManager.Instance.Current.SetActive();
            CameraSystem.Instance.MoveToCharacter(TeamManager.Instance.Current);
        }
    }

    public void Enable() { canSwitch = true; }
    public void Disable() { canSwitch = false; }
}
