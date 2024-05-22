using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayHandler : MonoBehaviour
{
    public static DisplayHandler Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        GameState.OnBeginEnemyTurn += Hide;
        GameState.OnBeginPlayerTurn += Show;
    }

    void Show()
    {
        transform.parent.gameObject.SetActive(true);
    }

    void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
