using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HandUIConfig _config;
    
    public GameObject SlotPrefab;
    public GameObject CardPrefab;
    RectTransform _rect;
    float _rectXMax;
    float _originalY;

    public List<Card> Cards;
    [SerializeField] Card _selected;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rectXMax = _rect.sizeDelta.x;
        _originalY = transform.position.y;
    }

    void Update()
    {
        if (Input.GetKeyDown("c")) AddDefaultCard();

        _rect.sizeDelta = new Vector2(Mathf.Clamp((Cards.Count + 1) * _config.SpacingPerCard, 100f, _rectXMax), _rect.sizeDelta.y); // Scale based on card count
    }

    void BeginDrag(Card card)
    {
        _selected = card;
    }

    void EndDrag(Card _)
    {
        if (_selected == null) return;

        _selected.transform.DOLocalMove(Vector3.zero, _config.ReturnTweenDuration);
        _selected = null;
    }

    void PointerEnter(Card _)
    {
        
    }
    
    void PointerExit(Card _)
    {
        
    }
    
    void PointerUp(Card _)
    {
        
    }
    
    void PointerDown(Card _)
    {
        
    }

    [ContextMenu("Add default card")]
    void AddDefaultCard()
    {
        GameObject slot = Instantiate(SlotPrefab, transform);
        Card card = Instantiate(CardPrefab, slot.transform).GetComponent<Card>(); // TODO Make it come from deck
        SubscribeEvents(card);
        Cards.Add(card);
    }
    
    void AddCard(Card card)
    {
        GameObject slot = Instantiate(SlotPrefab, transform);
        card.transform.SetParent(slot.transform);
        SubscribeEvents(card);
        Cards.Add(card);
    }

    void SubscribeEvents(Card card)
    {
        card.BeginDragEvent += BeginDrag;
        card.EndDragEvent += EndDrag;
        card.PointerEnterEvent += PointerEnter;
        card.PointerExitEvent += PointerExit;
        card.PointerUpEvent += PointerUp;
        card.PointerDownEvent += PointerDown;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_config.HoverScale, _config.HoverScaleDuration).SetEase(_config.HoverScaleEase);
        transform.DOMoveY(transform.position.y + _config.HoverOffset, _config.HoverScaleDuration).SetEase(_config.HoverScaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, _config.HoverScaleDuration).SetEase(_config.HoverScaleEase);
        transform.DOMoveY(_originalY, _config.HoverScaleDuration).SetEase(_config.HoverScaleEase);
    }
}