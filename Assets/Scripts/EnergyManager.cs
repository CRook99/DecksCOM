using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    static EnergyManager _instance;
    public static EnergyManager Instance { get { return _instance; } }
    
    public int Amount { get; private set; }
    [SerializeField] public int _maxAmount = 0;

    RectTransform _rectTransform;
    public Image _image;
    public TMP_Text _text;
    public AnimationCurve _wobbleCurve;
    public AnimationCurve _easeInCurve;
    float _originalPosX;

    void Awake()
    {
        _instance = this;
        _rectTransform = GetComponent<RectTransform>();
        _originalPosX = _rectTransform.anchoredPosition.x;

        Amount = _maxAmount;
        RefreshUI();
    }

    void OnEnable()
    {
        GameState.OnBeginPlayerTurn += TurnIncrease;
    }

    void OnDisable()
    {
        GameState.OnBeginEnemyTurn -= TurnIncrease;
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
        _text.text = Amount.ToString();
    }

    
    public IEnumerator InsufficientAnim()
    {
        _image.color = new Color32(255, 0, 0, 255);
        float elapsed = 0f;
        float factorPos = 0f;
        float factorCol = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            factorPos = _wobbleCurve.Evaluate(elapsed / duration);
            factorCol = _easeInCurve.Evaluate(elapsed / duration);
            _rectTransform.anchoredPosition = new Vector3(_originalPosX + (factorPos * 10), _rectTransform.anchoredPosition.y, 0f);
            _image.color = new Color32(255, (byte) (255 * factorCol), (byte) (255 * factorCol), 255);
            yield return null;
            elapsed += Time.deltaTime;
        }

        _image.color = new Color32(255, 255, 255, 255);
    }
}
