using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }
    
    public int Amount { get; private set; }
    [SerializeField] int _maxAmount;

    RectTransform _rectTransform;
    public Image Image;
    public TMP_Text Text;
    
    Sequence _sequence;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }
        
        _rectTransform = GetComponent<RectTransform>();

        Amount = _maxAmount;
        RefreshUI();
        
        GameState.OnBeginPlayerTurn += TurnIncrease;

        _sequence = DOTween.Sequence()
            .Append(Image.DOColor(new Color32(255, 0, 0, 255), 0f))
            .Append(_rectTransform.DOShakeAnchorPos(0.5f, new Vector2(15f, 0f), 25, 0))
            .Append(Image.DOColor(new Color32(255, 255, 255, 255), 0.5f))
            .SetAutoKill(false)
            .Pause();
    }

    
    public void Increase(int increase)
    {
        if (increase <= 0) return;
        Amount = Mathf.Clamp(Amount + increase, 0, _maxAmount);
        RefreshUI();
    }

    
    public void Decrease(int decrease)
    {
        if (decrease <= 0) return;
        Amount = Mathf.Clamp(Amount - decrease, 0, _maxAmount);
        RefreshUI();
    }

    
    public void TurnIncrease()
    {
        _maxAmount += 1;
        Amount = _maxAmount;
        RefreshUI();
    }

    
    void RefreshUI()
    {
        Text.text = Amount.ToString();
    }

    
    public void PlayInsufficientAnim()
    {
        _sequence.Restart();
    }
}
