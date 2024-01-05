using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeamManager : MonoBehaviour
{
    private static TeamManager _instance;
    public static TeamManager Instance { get { return _instance; } }

    [SerializeField] List<Player> _players;
    private Player _current;

    void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public Player Current { get { return _current; } }

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
