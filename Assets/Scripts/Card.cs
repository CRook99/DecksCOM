using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CardScriptableObject _cardData;
    private Vector2 _dragOffset;
    private Transform _handTransform;
    private GameObject _placeholder = null;

    public List<GameObject> hov;

    void Awake()
    {
        _handTransform = transform.parent;
        _cardData = GetComponent<CardDisplay>().card;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData = ExtendedStandaloneInputModule.GetPointerEventData();
        _dragOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        
        _placeholder = new GameObject();
        LayoutElement le = _placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        _placeholder.transform.SetParent(_handTransform);
        _placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        eventData = ExtendedStandaloneInputModule.GetPointerEventData();
        transform.position = eventData.position - _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (GameObject go in ExtendedStandaloneInputModule.Hovered)
        {
            if (go.GetComponent<PlayArea>() == null) continue;

            if (EnergyManager.Instance.Amount >= _cardData.Cost)
            {
                Use();
                return;
            }
            else 
            {
                StartCoroutine(EnergyManager.Instance.InsufficientAnim());
            }
        }

        transform.SetParent(_handTransform);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(_placeholder);
    }

    public void Use()
    {
        EnergyManager.Instance.Decrease(_cardData.Cost);
        Destroy(_placeholder);
        Destroy(gameObject);
    }
}
