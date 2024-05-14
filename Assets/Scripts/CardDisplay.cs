using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Card _card;
    public GameObject VisualsRoot;

    public Image Artwork;
    public TMP_Text CostText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;

    Vector2 _dragOffset;
    GameObject _placeholder;

    void Start()
    {
        _card = GetComponent<Card>();
        
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        Artwork.sprite = _card.Data.Artwork;
        CostText.text = _card.Data.Cost.ToString();
        NameText.text = _card.Data.Name;
        DescriptionText.text = _card.Data.Description;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData = ExtendedStandaloneInputModule.GetPointerEventData();
        _dragOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        
        _placeholder = new GameObject();
        Hand.Instance.AddTransformToHand(_placeholder.transform);
        LayoutElement le = _placeholder.AddComponent<LayoutElement>();
        
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
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

            if (EnergyManager.Instance.Amount >= _card.Data.Cost)
            {
                _card.Use();
                return;
            }
            
            StartCoroutine(EnergyManager.Instance.InsufficientAnim());
            
        }
        
        Hand.Instance.AddTransformToHand(transform);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(_placeholder);
    }

    public void Use()
    {
        Destroy(_placeholder);
        VisualsRoot.gameObject.SetActive(false);
    }
}
