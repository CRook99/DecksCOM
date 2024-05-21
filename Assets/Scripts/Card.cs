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
    DisplayHandler _displayHandler;

    Vector3 _offset;

    public bool IsHovering;
    public bool IsDragging;
    public bool WasDragged;
    
    public event Action<Card> BeginDragEvent;
    public event Action<Card> EndDragEvent;
    public event Action<Card> PointerEnterEvent;
    public event Action<Card> PointerExitEvent;
    public event Action<Card> PointerUpEvent;
    public event Action<Card> PointerDownEvent;

    
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
        

        _displayHandler = FindObjectOfType<DisplayHandler>();
        _display = Instantiate(DisplayPrefab, _displayHandler ? _displayHandler.transform : _canvas.transform).GetComponent<CardDisplay>();
        _display.Initialize(this);
    }

    void Update()
    {
        if (!IsDragging) return;
        
        // Cancel with right click
        if (Input.GetMouseButtonDown(1))
        {
            transform.position = Vector3.zero;
            OnEndDrag(null);
        }

        Vector2 target = Input.mousePosition - _offset;
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 velocity = direction * (Vector2.Distance(transform.position, target) / Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent?.Invoke(this);
        _offset = Input.mousePosition - transform.position; // TODO offset is weird
        IsDragging = true;
        WasDragged = true;
        _image.raycastTarget = false;
        _canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent?.Invoke(this);
        IsDragging = false;
        WasDragged = false;
        _image.raycastTarget = true;
        _canvas.GetComponent<GraphicRaycaster>().enabled = true;

        // StartCoroutine(Wait());
        //
        // IEnumerator Wait()
        // {
        //     yield return null;
        //     WasDragged = false;
        // }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent?.Invoke(this);
        IsHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent?.Invoke(this);
        IsHovering = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    
}
