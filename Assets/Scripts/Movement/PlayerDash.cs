using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    GridMovement _movement;
    [SerializeField] int _currentBonus;

    void Awake()
    {
        _movement = GetComponent<GridMovement>();
        _currentBonus = 0;
    }

    public int IncreaseBonus()
    {
        if (_currentBonus == 5)
            throw new Exception("Cannot dash past 5");
        
        _movement.IncrementBonus();
        _movement.CalculateSelectableTiles();
        _movement.ShowRange();

        _currentBonus++;
        return GetBonus();
    }

    public int GetBonus()
    {
        return _currentBonus;
    }

    public void ResetBonus()
    {
        _currentBonus = 0;
        _movement.ResetBonus();
    }
}
