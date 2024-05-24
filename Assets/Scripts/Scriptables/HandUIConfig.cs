using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "HandUIConfig", menuName = "Config/Hand UI Config")]
public class HandUIConfig : ScriptableObject
{
    public float SpacingPerCard;
    public float ReturnTweenDuration;
    public float HoverScale;
    public float HoverOffset;
    public float HoverScaleDuration;
    public Ease HoverScaleEase;

    public float AppearHeight;
    public float AppearDuration;
    public AnimationCurve AppearCurve;
}
