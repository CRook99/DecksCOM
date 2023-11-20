using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : Character
{
    private void Start()
    {
        gridMovement = gameObject.AddComponent<PlayerMovement>();
        healthManager = new HealthManager(100);
        movementRange = 9;
        SetMovementRange(movementRange);
    }
}
