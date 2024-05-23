using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelZoneScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static CancelZoneScript Instance { get; private set; }
    public Material Material;
    public float VisualDuration;
    
    Image _image;
    RectTransform _rect;
    public float Height;

    public bool IsHovered;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        Card.StaticBeginDragEvent += Show;
        Card.StaticEndDragEvent += Hide;
        
        _image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
        Height = _rect.rect.height;
        float aspectRatio = _rect.rect.width / _rect.rect.height;
        Material.SetFloat("_AspectRatio", aspectRatio);
        _image.material = Material;

        //Material.SetFloat("_Opacity", 0);
        _rect.anchoredPosition = new Vector2(0, -300);
    }

    void Update()
    {
        if (Input.GetKeyDown("v")) Hide();
    }

    void Show()
    {
        //Material.DOFloat(Material.GetFloat("_MaxOpacity"), "_Opacity", 0.5f).SetEase(Ease.OutCubic);
        _rect.DOAnchorPosY(0, VisualDuration).SetEase(Ease.OutQuint);
    }

    void Hide()
    {
        //Material.DOFloat(0, "_Opacity", 0.5f).SetEase(Ease.OutCubic);
        _rect.DOAnchorPosY(-300, VisualDuration).SetEase(Ease.OutQuint); // Magic 300
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
    }
}