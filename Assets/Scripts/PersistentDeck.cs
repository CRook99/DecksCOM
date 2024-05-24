using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDeck : MonoBehaviour
{
    [SerializeField] List<CardData> _cardDatas;

    void Update()
    {
        if (Input.GetKeyDown("l")) Deck.Instance.LoadCards(_cardDatas);
    }
}
