using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;
    public static GameState Instance { get { return _instance; } }

    private Turn turn;

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

        PassTurnToPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown("t")) SwitchTurn(); // DEBUGGING
    }

    public Turn Turn { get { return turn; } }

    void SwitchTurn()
    {
        if (turn == Turn.PLAYER) PassTurnToEnemy();
        else PassTurnToPlayer();
    }

    void PassTurnToPlayer()
    {
        turn = Turn.PLAYER;
        TeamManager.Instance.BeginTurn();
    }

    void PassTurnToEnemy()
    {
        turn = Turn.ENEMY;
    }

}
