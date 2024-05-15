using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashButtonUI : MonoBehaviour
{
    static DashButtonUI _instance;
    public static DashButtonUI Instance { get { return _instance; } }
    
    [SerializeField] TextMeshProUGUI _costText;
    int cost;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        
        ResetCost();
    }

    void OnEnable()
    {
        TeamSwitcher.OnSwitch += Refresh;
    }
    
    void OnDisable()
    {
        TeamSwitcher.OnSwitch -= Refresh;
    }

    public void Dash()
    {
        if (cost > EnergyManager.Instance.Amount) return;
        // IMPROVE - Chain could be better
        cost = TeamManager.Instance.Current.gameObject.GetComponent<PlayerDash>().IncreaseBonus();
        EnergyManager.Instance.Decrease(cost);
        UpdateCostForCurrentPlayer();
    }

    public void Refresh(int _)
    {
        UpdateCostForCurrentPlayer();
        if (cost > EnergyManager.Instance.Amount || !TeamManager.Instance.Current.CanMove)
        {
            // TODO - Fade logic
        }
    }

    void UpdateCostForCurrentPlayer()
    {
        cost = TeamManager.Instance.Current.gameObject.GetComponent<PlayerDash>().GetBonus() + 1;
        _costText.text = cost.ToString();
    }

    public void ResetCost()
    {
        cost = 1;
        _costText.text = cost.ToString();
    }
    
    
}
