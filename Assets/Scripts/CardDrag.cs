using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 _dragOffset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
