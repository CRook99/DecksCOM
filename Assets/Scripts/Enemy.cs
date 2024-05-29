using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    GridMovement _gridMovement;

    void Awake()
    {
        _gridMovement = GetComponent<GridMovement>();
    }

    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }
    
    public Tile GetCurrentTile()
    {
        return _gridMovement.GetCurrentTile();
    }
}
