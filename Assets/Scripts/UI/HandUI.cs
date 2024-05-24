using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Hand _hand;
    public HandUIConfig _config;
    
    public GameObject SlotPrefab;
    public GameObject CardPrefab;
    RectTransform _rect;
    float _rectXMax;
    float _originalY;

    public List<Card> Cards;
    Dictionary<Card, GameObject> _slotDict = new ();
    Card _selected;

    void Awake()
    {
        _hand = GetComponent<Hand>();
        _rect = GetComponent<RectTransform>();
        _rectXMax = _rect.sizeDelta.x;
        _originalY = transform.position.y;
    }


    void Update()
    {
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

    [ContextMenu("Add default card")]
    void AddDefaultCard()
    {
        GameObject slot = Instantiate(SlotPrefab, transform);
        Card card = Instantiate(CardPrefab, slot.transform).GetComponent<Card>(); // TODO Make it come from deck
        SubscribeEvents(card);
        Cards.Add(card);
    }
    
    public void AddCard(Card card)
    {
        GameObject slot = Instantiate(SlotPrefab, transform.position, Quaternion.identity);
        card.transform.SetParent(slot.transform);
        _slotDict.Add(card, slot);
        SubscribeEvents(card);
        StartCoroutine(CardAppear());
        
        IEnumerator CardAppear()
        {
            yield return slot.transform.DOLocalMoveY(_config.AppearHeight, _config.AppearDuration).SetEase(_config.AppearCurve).WaitForCompletion();
            slot.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            Cards.Add(card); // Placed here so width of hand rect is updated when it should be
        }
    }

    void UseCard(Card card)
    {
        GameObject slot = _slotDict[card];
        if (slot == null) throw new Exception($"No matching slot found for {card.Data.Name}");
        _slotDict.Remove(card);
        Destroy(slot); // TODO also destroys card, figure this out
    }
    

    void SubscribeEvents(Card card)
    {
        card.BeginDragEvent += BeginDrag;
        card.EndDragEvent += EndDrag;
        card.UseEvent += UseCard;
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
