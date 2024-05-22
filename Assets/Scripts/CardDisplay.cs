using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    bool _initialized;

    [SerializeField] CardDisplayConfig _cardDisplayConfig;
    [SerializeField] HandCurveConfig _handCurveConfig;
    
    Card _card;
    Transform _cardTransform;
    Canvas _canvas;
    
    Vector3 _movementDelta;
    Vector3 _rotationDelta;

    [SerializeField] float _curveYOffset;
    [SerializeField] float _curveRotOffset;

    public void Initialize(Card card)
    {
        _card = card;
        _cardTransform = _card.transform;
        _canvas = GetComponent<Canvas>();

        _card.BeginDragEvent += BeginDrag;
        _card.EndDragEvent += EndDrag;
        _card.PointerEnterEvent += PointerEnter;
        _card.PointerExitEvent += PointerExit;
        _card.PointerUpEvent += PointerUp;
        _card.PointerDownEvent += PointerDown;
        
        _initialized = true;
    }

    void Update()
    {
        if (!_initialized || _card == null) return;

        //HandPosition();
        FollowPosition();
        FollowRotation();
        
    }

    void FollowPosition()
    {
        Vector3 verticalOffset = Vector3.up * (_card.IsDragging ? 0 : _curveYOffset); // May be added
        transform.position =
            Vector3.Lerp(transform.position, _cardTransform.position, _cardDisplayConfig.FollowSpeed * Time.deltaTime);
    }

    void FollowRotation()
    {
        float movement = transform.position.x - _cardTransform.position.x;
        movement *= _cardDisplayConfig.RotationAmount * 0.01f; // Magic
        movement = Mathf.Clamp(movement, -45f, 45f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, movement);
    }

    // void HandPosition()
    // {
    //     _curveYOffset = _card.SiblingAmountIncl() > 4 ? _handCurveConfig.Positioning.Evaluate(_card.NormalizedPosition()) : 0;
    //     _curveRotOffset = _card.SiblingAmountIncl() > 4 ? _handCurveConfig.Rotation.Evaluate(_card.NormalizedPosition()) : 0;
    // }

    void BeginDrag(Card _)
    {
        transform.DOScale(_cardDisplayConfig.ScaleOnHover, _cardDisplayConfig.Transition).SetEase(_cardDisplayConfig.ScaleEase);
        _canvas.overrideSorting = true;
    }

    void EndDrag(Card _)
    {
        transform.DOScale(1, _cardDisplayConfig.Transition).SetEase(_cardDisplayConfig.ScaleEase);
        _canvas.overrideSorting = false;
    }

    void PointerEnter(Card _)
    {
        transform.DOScale(_cardDisplayConfig.ScaleOnHover, _cardDisplayConfig.Transition).SetEase(_cardDisplayConfig.ScaleEase);
        //transform.DOMoveY(transform.position.y + _cardDisplayConfig.OffsetOnHover, _cardDisplayConfig.Transition).SetEase(_cardDisplayConfig.OffsetEase);
        _canvas.overrideSorting = true;
    }

    void PointerExit(Card _)
    {
        if (!_card.WasDragged) transform.DOScale(1, _cardDisplayConfig.Transition).SetEase(_cardDisplayConfig.ScaleEase);
        //transform.DOMoveY(transform.position.y - _cardDisplayConfig.OffsetOnHover, _cardDisplayConfig.Transition).SetEase(_cardDisplayConfig.OffsetEase);
        _canvas.overrideSorting = false;
    }

    void PointerUp(Card _)
    {
        
    }

    void PointerDown(Card _)
    {
        
    }
    
}
