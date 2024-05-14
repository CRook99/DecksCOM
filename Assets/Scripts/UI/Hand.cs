using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance { get; private set; }

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
        if (Input.GetKeyDown("p"))
        {
            DebugDrawCard();
        }
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

    void DebugDrawCard()
    {
        GameObject o = Instantiate(DefaultCard, transform);
        Cards.Add(o.GetComponent<Card>());
    }
    
    
}
