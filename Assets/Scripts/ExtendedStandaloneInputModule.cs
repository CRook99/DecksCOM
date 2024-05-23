using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtendedStandaloneInputModule : StandaloneInputModule
{
    public static ExtendedStandaloneInputModule Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public static PointerEventData GetPointerEventData(int pointerId = -1)
    {
        PointerEventData eventData;
        Instance.GetPointerData(pointerId, out eventData, true);
        return eventData;
    }

    public static List<GameObject> Hovered { get { return GetPointerEventData().hovered; } }
}
