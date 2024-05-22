using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Card/Weapon")]
public class WeaponSO : CardScriptableObject
{
    public int Hits;
    public int Targets;
    public int Range;
}