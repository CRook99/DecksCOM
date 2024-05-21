using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    //public CardScriptableObject Data;
    CardDisplay _display;
    public GameObject DisplayPrefab;

    Canvas _canvas;
    Image _image;
    RectTransform _rectTransform;

    Vector3 _offset;

    public bool isHovering;
    public bool isDragging;
    
    public event Action BeginDragEvent;
    public event Action EndDragEvent;
    public event Action PointerEnterEvent;
    public event Action PointerExitEvent;
    public event Action PointerUpEvent;
    public event Action PointerDownEvent;

    
    // void Awake()
    // {
    //     _display = GetComponent<CardDisplay>();
    // }

    // public void Use()
    // {
    //     Debug.Log($"Used {Data.Name} for {Data.Cost} energy");
    //     EnergyManager.Instance.Decrease(Data.Cost);
    //     
    //     _display.Use();
    //     Hand.Instance.RemoveCardFromHand(this);
    //     DiscardPile.Instance.AddCardToPile(this);
    // }

    void Start()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        _display = Instantiate(DisplayPrefab, _canvas.transform).GetComponent<CardDisplay>();
        _display.Initialize(this);
    }

    void Update()
    {
        if (!isDragging) return;

        Vector2 target = Input.mousePosition - _offset;
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 velocity = direction * (Vector2.Distance(transform.position, target) / Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent?.Invoke();
        _offset = Input.mousePosition - transform.position; // TODO offset is weird
        isDragging = true;
        _image.raycastTarget = false;
        _canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent?.Invoke();
        isDragging = false;
        _image.raycastTarget = true;
        _canvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent?.Invoke();
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent?.Invoke();
        isHovering = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
