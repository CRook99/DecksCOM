using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    private static EnergyManager _instance;
    public static EnergyManager Instance { get { return _instance; } }

    private int _amount;
    private int _maxAmount = 10;

    private RectTransform _rectTransform;
    public Image _image;
    public TMP_Text _text;
    public AnimationCurve _wobbleCurve;
    public AnimationCurve _easeInCurve;
    private float _originalPosX;

    void Awake()
    {
        _instance = this;
        _rectTransform = GetComponent<RectTransform>();
        _originalPosX = _rectTransform.anchoredPosition.x;
        _amount = _maxAmount;
        _text.text = _amount.ToString();
    }

    public int Amount { get { return _amount; } }

    public void Increase(int increase)
    {
        if (increase <= 0) return;
        _amount = Mathf.Clamp(_amount + increase, 0, _maxAmount);
        _text.text = _amount.ToString();
    }

    public void Decrease(int decrease)
    {
        if (decrease <= 0) return;
        _amount = Mathf.Clamp(_amount - decrease, 0, _maxAmount);
        _text.text = _amount.ToString();
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
