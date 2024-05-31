using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Card/Weapon")]
public class WeaponData : CardData
{
    [Header("Weapon Stats")]
    public int Damage;
    public int Range = 9;
    public int Targets = 1;
    public int SplashRadius = 0;
    public bool IgnoreCover = false;
}