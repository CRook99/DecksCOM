using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private int movementRange = 8;

    private void Start()
    {
        playerMovement = gameObject.AddComponent<PlayerMovement>();
        playerMovement.SetMovementRange(movementRange);
    }

    public void SetMovementRange(int range)
    {
        movementRange = range;
        playerMovement.SetMovementRange(movementRange);
    }
}
