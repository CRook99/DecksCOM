using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    private HealthManager _healthManager;

    [SerializeField] Image _fill;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _gradient;
    Color _greenColor = new Color32(0, 255, 0, 255);
    Color _redColor = new Color32(255, 0, 0, 255);

    public void Initialize(HealthManager manager)
    {
        if (manager == null) return;
        _healthManager = manager;

        _fill.fillAmount = 1;
        _text.text = _healthManager.Health.ToString();
        _gradient.enabled = false;
    }

    public void UpdateValues()
    {
        _fill.fillAmount = _healthManager.NormalizedHealth();
        _fill.color = (_healthManager.NormalizedHealth() > 0.25f) ? _greenColor : _redColor;
        _text.text = _healthManager.Health.ToString();        
    }

    public void UpdateIndicator(bool b)
    {
        if (b)
        {
            _gradient.enabled = true;
            _gradient.rectTransform.anchoredPosition = Vector3.zero;
            _gradient.rectTransform.DOLocalMoveX(100f, 0.25f).SetEase(Ease.OutBack);
        }
        else
        {
            _gradient.rectTransform.DOLocalMoveX(-100f, 0.1f);
            _gradient.enabled = false;
        }
    }
}
