using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public GridMovement gridMovement;
    protected HealthManager healthManager;
    protected int movementRange;
    protected int maxHealth;

    public void SetMovementRange(int range)
    {
        movementRange = range;
        gridMovement.SetMovementRange(movementRange);
    }
}
