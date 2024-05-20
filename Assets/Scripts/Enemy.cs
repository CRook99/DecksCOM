using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }
}
