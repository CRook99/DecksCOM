using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    public GameObject CardPrefab;
    List<Card> _cards;
    RectTransform _rect;

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

        _rect = GetComponent<RectTransform>();
        _cards = new List<Card>();
    }

    public void LoadCards(List<CardData> cardDatas)
    {
        foreach (CardData data in cardDatas)
        {
            Card card = Instantiate(CardPrefab, transform).GetComponent<Card>();
            card.transform.localPosition = Vector3.zero;
            card.LoadData(data);
            _cards.Add(card);
        }
    }

    public void AddCardToDeck(Card card)
    {
        if (card == null) return;
        _cards.Add(card);
    }

    public Card Draw()
    {
        if (_cards.Count == 0) return null;
        Card c = _cards[0];
        _cards.RemoveAt(0);
        //Debug.Log($"Drew card {c.Data.Name}");
        return c;
    }

    public Card GetRandomCard()
    {
        return _cards.Count > 0 ? _cards[Random.Range(0, _cards.Count)] : null;
    }
}
