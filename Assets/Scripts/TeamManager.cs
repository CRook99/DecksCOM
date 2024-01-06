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

    void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public Player Current { get { return _current; } }

    public void SetCurrent(int index)
    {
        if (index < 0 || index > 2) index = 0;
        _current = _players[index];
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
