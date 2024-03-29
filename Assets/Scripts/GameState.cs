using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;
    public static GameState Instance { get { return _instance; } }

    private Turn _turn;
    private int _turnCount;

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

        _turnCount = 0;
        PassTurnToPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown("t")) SwitchTurn(); // DEBUGGING
    }

    public Turn Turn { get { return _turn; } }

    void SwitchTurn()
    {
        if (_turn == Turn.PLAYER) PassTurnToEnemy();
        else PassTurnToPlayer();
    }

    void PassTurnToPlayer()
    {
        _turn = Turn.PLAYER;
        TeamManager.Instance.BeginTurn();
        EnergyManager.Instance.TurnIncrease();
    }

    void PassTurnToEnemy()
    {
        _turn = Turn.ENEMY;
        _turnCount += 1;
    }

}
