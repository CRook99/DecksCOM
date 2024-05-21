using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    bool _initialized;

    [SerializeField] CardDisplayConfig _config;
    
    Card _card;
    Transform _cardTransform;
    
    Vector3 _movementDelta;
    Vector3 _rotationDelta;

    public void Initialize(Card card)
    {
        _card = card;
        _cardTransform = _card.transform;

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

        FollowPosition();
        FollowRotation();
    }

    void FollowPosition()
    {
        transform.position =
            Vector3.Lerp(transform.position, _cardTransform.position, _config.FollowSpeed * Time.deltaTime);
    }

    void FollowRotation()
    {
        float movement = transform.position.x - _cardTransform.position.x;
        movement *= _config.RotationAmount * 0.01f; // Magic
        movement = Mathf.Clamp(movement, -45f, 45f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, movement);
    }

    void BeginDrag()
    {
        
    }

    void EndDrag()
    {
        
    }

    void PointerEnter()
    {
        
    }

    void PointerExit()
    {
        
    }

    void PointerUp()
    {
        
    }

    void PointerDown()
    {
        
    }
    
}
