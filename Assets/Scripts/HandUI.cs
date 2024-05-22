using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    public GameObject SlotPrefab;
    public GameObject CardPrefab;
    RectTransform _rect;
    float _rectXMax;

    public List<Card> Cards;
    [SerializeField] Card _selected;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rectXMax = _rect.sizeDelta.x;
    }

    void Update()
    {
        if (Input.GetKeyDown("c")) AddDefaultCard();

        _rect.sizeDelta = new Vector2(Mathf.Clamp((Cards.Count + 1) * 100, 100f, _rectXMax), _rect.sizeDelta.y); // Scale based on card count
    }

    void BeginDrag(Card card)
    {
        _selected = card;
    }

    void EndDrag(Card _)
    {
        if (_selected == null) return;

        _selected.transform.DOLocalMove(Vector3.zero, 0.15f);
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
}
