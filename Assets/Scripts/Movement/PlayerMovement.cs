using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GridMovement
{
    void Start()
    {
        Initialize();
        SetMovement(8);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) FindSelectableTiles();
    }




}
