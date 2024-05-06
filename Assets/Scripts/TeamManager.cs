using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeamManager : MonoBehaviour
{
    private static TeamManager _instance;
    public static TeamManager Instance { get { return _instance; } }

    [SerializeField] List<Player> _players;
    [SerializeField] Player _current;
    [SerializeField] Player _previous;

    void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
        _current = _players[0];
    }

    public Player Current { get { return _current; } }
    public Player Previous { get { return _previous; } }

    public void SetCurrent(int index)
    {
        if (index < 0 || index > 2) index = 0;
        _previous = _current;
        _current = _players[index];
        _current.SetActive();
    }

    public void BeginTurn()
    {
        foreach (Player player in _players)
        {
            player.BeginTurn();
        }
        
        SetCurrent(0);
        DashButtonUI.Instance.UpdateCostForCurrentPlayer();
    }

    public int GetAllPlayerCount() { return _players.Count; }

    public int GetAlivePlayerCount()
    {
        return _players.Count(p => !p.IsDead());
    }

    public Player GetPlayerByIndex(int index)
    {
        return _players[index];
    }
}
