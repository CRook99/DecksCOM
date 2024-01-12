using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CardScriptableObject _cardData;
    private Vector2 _dragOffset;
    private Transform _handTransform;

    void Awake()
    {
        _handTransform = transform.parent;
        _cardData = GetComponent<CardDisplay>().card;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(transform.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Use()
    {
        Debug.Log($"{_cardData.Name} got used for {_cardData.Cost} energy.");
    }
}
