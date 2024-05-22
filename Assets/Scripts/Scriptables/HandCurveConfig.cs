using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandCurveConfig", menuName = "Config/Hand Curve Config")]
public class HandCurveConfig : ScriptableObject
{
    public AnimationCurve Positioning;
    public float PositioningInfluence;
    public AnimationCurve Rotation;
    public float RotationInfluence;
}
