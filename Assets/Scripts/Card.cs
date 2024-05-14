using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Card : MonoBehaviour
{
    public CardScriptableObject Data;
    CardDisplay _display;

    void Awake()
    {
        _display = GetComponent<CardDisplay>();
    }

    public void Use()
    {
        Debug.Log($"Used {Data.Name} for {Data.Cost} energy");
        EnergyManager.Instance.Decrease(Data.Cost);
        
        _display.Use();
        Hand.Instance.RemoveCardFromHand(this);
        DiscardPile.Instance.AddCardToPile(this);
    }
}
