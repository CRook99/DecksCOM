using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    Image _image;
    float _rotateDuration = 5f;
    float _shrinkDuration = 0.5f;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    void Start()
    {
        _image.rectTransform.DORotate(new Vector3(0f, 0f, -360f), _rotateDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void ShrinkAnim()
    {
        DOTween.Kill("Shrink");
        _image.rectTransform.localScale = Vector3.one * 2f;
        _image.rectTransform.DOScale(Vector3.one, _shrinkDuration).SetEase(Ease.OutCubic).SetId("Shrink");
    }
}
