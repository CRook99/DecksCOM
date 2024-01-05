using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    private static TeamManager _instance;
    public static TeamManager Instance { get { return _instance; } }

    private List<Player> _players;

    void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;

        _players = new List<Player>();
    }
}
