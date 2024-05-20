using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [SerializeField] List<Enemy> _enemies;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        _enemies.Add(enemy);
    }

    public List<Enemy> GetEnemies()
    {
        return _enemies;
    }
}
