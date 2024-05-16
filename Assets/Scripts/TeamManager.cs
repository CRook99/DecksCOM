using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeamManager : MonoBehaviour
{
    private static TeamManager _instance;
    public static TeamManager Instance { get { return _instance; } }

    [SerializeField] List<Player> _players;

    void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    void OnEnable()
    {
        GameState.OnBeginPlayerTurn += BeginTurn;
    }

    void OnDisable()
    {
        GameState.OnBeginPlayerTurn -= BeginTurn;
    }

    public Player Current { get; private set; }
    public Player Previous { get; private set; }

    public void AddPlayer(Player player)
    {
        if (player == null) return;
        _players.Add(player);
    }

    public void SetCurrent(int index)
    {
        if (Current != null) Current.SetInactive();
        if (index < 0 || index > 2) index = 0;
        Previous = Current;
        Current = _players[index];
        Current.SetActive();
    }

    public void BeginTurn()
    {
        SetCurrent(0);
        StartCoroutine(CameraSystem.Instance.MoveToPoint(Current.gameObject));
    }

    public int GetAllPlayerCount() { return _players.Count; }

    public int GetAlivePlayerCount()
    {
        return _players.Count(p => !p.Dead);
    }

    public Player GetPlayerByIndex(int index)
    {
        return _players[index];
    }
}
