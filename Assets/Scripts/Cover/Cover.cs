using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Cover : MonoBehaviour
{
    [SerializeField] protected CoverLevel level;

    [ContextMenu("Get Cover")]
    public float GetCover() 
    { 
        switch (level)
        {
            case CoverLevel.FULL: return 1f;
            case CoverLevel.HALF: return 0.5f;
            case CoverLevel.NONE: return 0f;
            default: return 0f;
        }
    }
}
