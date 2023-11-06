using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GridMovement
{
    void Start()
    {
        Initialize();
        SetMovementRange(8);
        SetMovementSpeed(4);
    }

    

}
