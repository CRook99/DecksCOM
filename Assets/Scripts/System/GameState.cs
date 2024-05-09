using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameState : MonoBehaviour
{
    static GameState _instance;
    public static GameState Instance { get { return _instance; } }

    public static event Action OnBeginPlayerTurn;
    public static event Action OnBeginEnemyTurn;
    
    int _turnCount;
    
    public Turn CurrentTurn { get; private set; }

    public TextMeshProUGUI TurnText;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        _turnCount = 0;
        PassTurnToEnemy();
    }

    void Update()
    {
        if (Input.GetKeyDown("t")) SwitchTurn(); // DEBUGGING
    }

    void SwitchTurn()
    {
        if (CurrentTurn == Turn.PLAYER) PassTurnToEnemy();
        else PassTurnToPlayer();
    }

    void PassTurnToPlayer()
    {
        CurrentTurn = Turn.PLAYER;
        OnBeginPlayerTurn?.Invoke();

        TurnText.text = "TURN: PLAYER";
    }

    void PassTurnToEnemy()
    {
        CurrentTurn = Turn.ENEMY;
        OnBeginEnemyTurn?.Invoke();
        _turnCount += 1;
        
        TurnText.text = "TURN: ENEMY";
    }

}
