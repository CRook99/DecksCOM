using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class CardDisplayConfig
{
    [Header("Follow")]
    public float FollowSpeed = 5f;

    [Header("Rotation")] 
    public float RotationAmount = 20;

    [Header("Scale")] 
    public float ScaleOnHover = 1.15f;
    public float ScaleTransition = 0.15f;
    public Ease ScaleEase = Ease.OutBack;

}
