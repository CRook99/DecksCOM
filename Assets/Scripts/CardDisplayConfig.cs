using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CardDisplayConfig", menuName = "Config/Card Display Config")]
public class CardDisplayConfig : ScriptableObject
{
    [Header("Follow")]
    public float FollowSpeed = 5f;

    [Header("Rotation")] 
    public float RotationAmount = 20;

    [Header("Scale")] 
    public float ScaleOnHover = 1.15f;
    public Ease ScaleEase = Ease.OutBack;

    [Header("Hover")] 
    public float OffsetOnHover = 1f;
    public Ease OffsetEase = Ease.OutCubic;
    
    public float Transition = 0.15f;
}
