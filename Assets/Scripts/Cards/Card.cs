using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public CardData Data;
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
    bool _shouldUse; // Prevent usage with cancel OnEndDrag
    
    public event Action<Card> BeginDragEvent;
    public event Action<Card> EndDragEvent;
    public event Action<Card> PointerEnterEvent;
    public event Action<Card> PointerExitEvent;
    public event Action<Card> PointerUpEvent;
    public event Action<Card> PointerDownEvent;
    public event Action<Card> UseEvent;
    
    public static event Action StaticBeginDragEvent;
    public static event Action StaticEndDragEvent;

    PlayerMovementManager _playerMovementManager;
    

    public void LoadData(CardData data)
    {
        Data = data;
    }

    void Use()
    {
        Debug.Log($"Used {Data.Name} for {Data.Cost} energy");
        EnergyManager.Instance.Decrease(Data.Cost);
        UseEvent?.Invoke(this);

        switch (Data)
        {
            case WeaponData d:
                TargetingSystem.Instance.EnterTargeting(d);
                break;
            default:
                Debug.Log($"Give {name} a proper fucking Data object!");
                break;
        }
        
        Discard();
    }
    
    void Discard()
    {
        //Hand.Instance.RemoveCardFromHand(this);
        DiscardPile.Instance.AddCardToPile(this);
        _display.Hide();
    }

    void Awake()
    {
        _playerMovementManager = FindObjectOfType<PlayerMovementManager>();
    }

    void Start()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        

        _displayHandler = DisplayHandler.Instance;
        _display = Instantiate(DisplayPrefab, transform.position, Quaternion.identity, _displayHandler ? _displayHandler.transform : _canvas.transform).GetComponent<CardDisplay>();
        _display.Initialize(this);
        _display.UpdateVisuals(Data);
    }

    void Update()
    {
        if (!IsDragging) return;
        
        // Cancel with right click
        if (Input.GetMouseButtonDown(1))
        {
            //transform.position = Vector3.zero;
            _shouldUse = false;
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
        StaticBeginDragEvent?.Invoke();
        _offset = Input.mousePosition - transform.position; // TODO offset is weird
        IsDragging = true;
        WasDragged = true;
        IsHovering = false;
        _shouldUse = true;
        _image.raycastTarget = false;
        _canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.mousePosition.y < 25f)
        {
            _shouldUse = false;
            OnEndDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent?.Invoke(this);
        StaticEndDragEvent?.Invoke();
        IsDragging = false;
        WasDragged = false;
        IsHovering = false;
        _image.raycastTarget = true;
        _canvas.GetComponent<GraphicRaycaster>().enabled = true;

        if (!_shouldUse)
        {
            _playerMovementManager.Enable();
            return;
        }
        
        if (EnergyManager.Instance.Amount >= Data.Cost)
        {
            Use();
            StaticEndDragEvent?.Invoke();
        }
        else
        {
            EnergyManager.Instance.PlayInsufficientAnim();
            _playerMovementManager.Enable();
        }
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
    
    // public float NormalizedPosition()
    // {
    //     if (!transform.parent.CompareTag("Slot")) return -1;
    //
    //     int slotIndex = transform.parent.GetSiblingIndex();
    //     int slotAmount = transform.parent.parent.childCount;
    //     Debug.Log($"INDEX: {slotIndex} AMOUNT: {slotAmount}");
    //     return (float)slotIndex / (slotAmount - 1);
    // }

    // public int SiblingIndex()
    // {
    //     return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    // }
    //
    // public int SiblingAmountIncl()
    // {
    //     return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount : 1;
    // }
}
