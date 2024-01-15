using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    private static EnergyManager _instance;
    public static EnergyManager Instance { get { return _instance; } }

    private int _amount;
    private int _maxAmount = 1;

    void Awake()
    {
        _instance = this;
        _amount = _maxAmount;
    }

    public int Amount { get { return _amount; } }

    public void Increase(int increase)
    {
        if (increase <= 0) return;
        _amount = Mathf.Clamp(_amount + increase, 0, _maxAmount);
    }

    public void Decrease(int decrease)
    {
        if (decrease <= 0) return;
        _amount = Mathf.Clamp(_amount - decrease, 0, _maxAmount);
    }
}
