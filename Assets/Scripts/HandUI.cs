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
        _selected = null;
    }

    [ContextMenu("Add default card")]
    void AddDefaultCard()
    {
        GameObject slot = Instantiate(SlotPrefab, transform);
        Card card = Instantiate(CardPrefab, slot.transform).GetComponent<Card>();
        card.BeginDragEvent += BeginDrag;
        card.EndDragEvent += EndDrag;
        _cards.Add(card.GetComponent<Card>());
    }
    
    void AddCard(Card card)
    {
        GameObject slot = Instantiate(SlotPrefab, transform);
        slot.transform.SetParent(gameObject.transform);
        card.transform.SetParent(slot.transform);
        card.BeginDragEvent += BeginDrag;
        card.EndDragEvent += EndDrag;
        _cards.Add(card);
    }
}
