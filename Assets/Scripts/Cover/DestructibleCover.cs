using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCover : Cover
{
    [SerializeField] Mesh fullMesh;
    [SerializeField] Mesh halfMesh;
    [SerializeField] Mesh noneMesh;

    public void Weaken()
    {
        switch (level)
        {
            case CoverLevel.FULL:
                level = CoverLevel.HALF;
                break;
            case CoverLevel.HALF:
                level = CoverLevel.NONE;
                break;
            default: 
                break;
        }
    }
}
