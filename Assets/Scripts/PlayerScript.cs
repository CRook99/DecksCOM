using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private HealthManager healthManager;
    private int movementRange = 8;

    private void Start()
    {
        playerMovement = gameObject.AddComponent<PlayerMovement>();
        healthManager = new HealthManager(100);
        playerMovement.SetMovementRange(movementRange);

    }

    public void SetMovementRange(int range)
    {
        movementRange = range;
        playerMovement.SetMovementRange(movementRange);
    }
}
