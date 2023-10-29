using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GridMovement
{
    private bool isMoving;
    void Start()
    {
        Initialize();
        SetMovement(8);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) FindSelectableTiles();
    }

    private void CheckMouse()
    {

    }

}
