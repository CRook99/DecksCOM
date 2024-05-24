using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance { get; private set; } // IMPROVE Remove this
    public HandUI UI;
    public GameObject DefaultCard;

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

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            DrawCard();
        }
    }

    void DrawCard()
    {
        Card card = Deck.Instance.Draw();
        if (card == null) throw new NullReferenceException("Drew null card");
        Cards.Add(card);
        UI.AddCard(card);
    }

    public int Count()
    {
        return Cards.Count;
    }

    public void AddCardToHand(Card card)
    {
        if (card == null) return;
        Cards.Add(card);
        AddTransformToHand(card.gameObject.transform);
    }

    public void AddTransformToHand(Transform t)
    {
        t.SetParent(transform);
    }

    public void RemoveCardFromHand(Card card)
    {
        if (card == null) return;
        Cards.Remove(card);
    }

    void DebugDrawCard()
    {
        GameObject o = Instantiate(DefaultCard, transform);
        Cards.Add(o.GetComponent<Card>());
    }
    
    
}
