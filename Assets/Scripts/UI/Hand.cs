using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance { get; private set; }

    public GameObject DefaultCard;

    public List<Card> Cards;

    void Awake()
    {
        Instance = this;
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

    void DebugDrawCard()
    {
        
    }
    
    
}
