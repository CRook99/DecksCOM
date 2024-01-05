using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum Turn
    {
        PLAYER,
        ENEMY
    }

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
        if (Input.GetKeyDown("t")) SwitchTurn();
    }

    void SwitchTurn()
    {
        turn = (turn == Turn.PLAYER) ? Turn.ENEMY : Turn.PLAYER;
        Debug.Log(turn);
    }

    void PassTurnToPlayer()
    {
        turn = Turn.ENEMY;
    }

    void PassTurnToEnemy()
    {
        turn = Turn.ENEMY;
    }

}
