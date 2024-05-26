using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public List<GameObject> Elements;

    void OnEnable()
    {
        GameState.OnBeginPlayerTurn += Show;
        GameState.OnBeginEnemyTurn += Hide;
        TargetingSystem.OnEnterTargeting += Hide;
        TargetingSystem.OnExitTargeting += Show;
    }
    
    void OnDisable()
    {
        GameState.OnBeginPlayerTurn -= Show;
        GameState.OnBeginEnemyTurn -= Hide;
        TargetingSystem.OnEnterTargeting -= Hide;
        TargetingSystem.OnExitTargeting -= Show;
    }

    public void Show()
    {
        foreach (GameObject e in Elements)
        {
            e.SetActive(true);
        }
    }
    
    public void Hide()
    {
        foreach (GameObject e in Elements)
        {
            e.SetActive(false);
        }
    }
}
