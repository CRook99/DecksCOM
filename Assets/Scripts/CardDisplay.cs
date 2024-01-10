using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Image _artwork;
    public TMP_Text _costText;
    public TMP_Text _nameText;
    public TMP_Text _descriptionText;

    void Start()
    {
        _artwork.sprite = card.Artwork;
        _costText.text = card.Cost.ToString();
        _nameText.text = card.Name;
        _descriptionText.text = card.Description;
    }
}
