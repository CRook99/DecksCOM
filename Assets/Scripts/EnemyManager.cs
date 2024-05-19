using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> _enemies;

    public List<Enemy> GetEnemies()
    {
        return _enemies;
    }
}
