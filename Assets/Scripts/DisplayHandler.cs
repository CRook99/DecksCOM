using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHandler : MonoBehaviour
{
    public static DisplayHandler Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
}
