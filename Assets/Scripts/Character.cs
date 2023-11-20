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

    public void TakeDamage(int amount)
    {
        if (!healthManager.TakeDamage(amount))
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        healthManager.Heal(amount);
    }

    public abstract void Die();
}
