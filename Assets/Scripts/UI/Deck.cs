using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    public GameObject CardPrefab;
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

    public void LoadCards(List<CardData> cardDatas)
    {
        foreach (CardData data in cardDatas)
        {
            Card card = Instantiate(CardPrefab, transform).GetComponent<Card>();
            card.LoadData(data);
            Cards.Add(card);
        }
    }

    public void AddCardToDeck(Card card)
    {
        if (card == null) return;
        Cards.Add(card);
    }

    public Card Draw()
    {
        if (Cards.Count == 0) return null;
        Card c = Cards[0];
        Cards.RemoveAt(0);
        //Debug.Log($"Drew card {c.Data.Name}");
        return c;
    }

    public Card GetRandomCard()
    {
        return Cards.Count > 0 ? Cards[Random.Range(0, Cards.Count)] : null;
    }
}
