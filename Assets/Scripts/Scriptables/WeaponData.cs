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

// [CustomEditor(typeof(WeaponData))]
// public class WeaponDataEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         var script = (WeaponData)target;
//         script.Damage = EditorGUILayout.IntField("Damage", script.Damage);
//         script.Range = EditorGUILayout.IntField("Range", script.Range);
//
//         script.MultiTarget = EditorGUILayout.Toggle("Multi-target", script.MultiTarget);
//         script.Targets = script.MultiTarget ? EditorGUILayout.IntField("Targets", script.Targets) : 1;
//         
//         
//         script.Splash = EditorGUILayout.Toggle("Splash", script.Splash);
//         script.SplashRadius = script.Splash ? EditorGUILayout.IntField("Splash radius", script.SplashRadius) : 0;
//     }
// }