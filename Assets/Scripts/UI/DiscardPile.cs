using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public static DiscardPile Instance { get; private set; }
    
    public List<Card> Cards;

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
    }

    public void AddCardToPile(Card card)
    {
        if (card == null) return;
        Cards.Add(card);
    }
}
