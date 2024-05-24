using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance { get; private set; } // IMPROVE Remove this
    HandUI _UI;
    public GameObject DefaultCard;
    
    public int CardCount => _cards.Count;

    List<Card> _cards;

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

        _cards = new List<Card>();
        _UI = GetComponent<HandUI>();
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
        _cards.Add(card);
        card.UseEvent += RemoveCardFromHand; // IMPROVE Will change if decide to add cancelling
        _UI.AddCard(card);
    }

    public void AddCardToHand(Card card)
    {
        if (card == null) return;
        _cards.Add(card);
        AddTransformToHand(card.gameObject.transform);
    }

    public void AddTransformToHand(Transform t)
    {
        t.SetParent(transform);
    }

    public void RemoveCardFromHand(Card card)
    {
        if (card == null) return;
        _cards.Remove(card);
    }
}
