using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] Card hoveredCard;

    public GameObject SlotPrefab;
    public GameObject CardPrefab;
    RectTransform _rect;

    public List<Card> _cards;
    [SerializeField] Card _selected;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        //_cards = GetComponentsInChildren<Card>().ToList();

        
        
        // foreach (Card card in _cards)
        // {
        //     card.BeginDragEvent += BeginDrag;
        //     card.EndDragEvent += EndDrag;
        //     card.PointerEnterEvent += PointerEnter;
        //     card.PointerExitEvent += PointerExit;
        //     card.PointerUpEvent += PointerUp;
        //     card.PointerDownEvent += PointerDown;
        // }
    }

    void Update()
    {
        if (Input.GetKeyDown("c")) AddDefaultCard();
    }

    void BeginDrag(Card card)
    {
        _selected = card;
    }

    void EndDrag(Card _)
    {
        if (_selected == null) return;

        _selected.transform.DOLocalMove(Vector3.zero, 0.15f);
        // _rect.sizeDelta += Vector2.right;
        // _rect.sizeDelta -= Vector2.right;
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
        Card card = Instantiate(CardPrefab, slot.transform).GetComponent<Card>();
        SubscribeEvents(card);
        _cards.Add(card);
    }
    
    void AddCard(Card card)
    {
        GameObject slot = Instantiate(SlotPrefab, transform);
        card.transform.SetParent(slot.transform);
        SubscribeEvents(card);
        _cards.Add(card);
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
