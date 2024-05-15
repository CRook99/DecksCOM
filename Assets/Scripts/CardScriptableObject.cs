using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "Card/DefaultCard")]
public class CardScriptableObject : ScriptableObject 
{
    public Sprite Artwork;
    public int Cost;
    public string Name;
    public string Description;
}