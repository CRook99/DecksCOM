using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSwitcher : MonoBehaviour
{
    [SerializeField] CameraSystem cameraSystem; // Could be singleton
    
    void Update()
    {
        if (Input.GetKeyDown("1") && !TeamManager.Instance.GetPlayerByIndex(0).IsDead())
        {
            TeamManager.Instance.Current.SetInactive();
            TeamManager.Instance.SetCurrent(0);
            TeamManager.Instance.Current.SetActive();
            cameraSystem.MoveToCharacter(TeamManager.Instance.Current);
            
        }
        else if (Input.GetKeyDown("2") && !TeamManager.Instance.GetPlayerByIndex(1).IsDead())
        {
            TeamManager.Instance.Current.SetInactive();
            TeamManager.Instance.SetCurrent(1);
            TeamManager.Instance.Current.SetActive();
            cameraSystem.MoveToCharacter(TeamManager.Instance.Current);
        }
        else if (Input.GetKeyDown("3") && !TeamManager.Instance.GetPlayerByIndex(2).IsDead())
        {
            TeamManager.Instance.Current.SetInactive();
            TeamManager.Instance.SetCurrent(2);
            TeamManager.Instance.Current.SetActive();
            cameraSystem.MoveToCharacter(TeamManager.Instance.Current);
        }
    }
}
