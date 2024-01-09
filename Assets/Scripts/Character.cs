using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public GridMovement gridMovement;
    protected HealthManager healthManager;
    protected int movementRange;
    protected int maxHealth;
    protected bool canMove;
    public bool CanMove { get { return canMove; } }
    protected bool dead;

    public void BeginTurn()
    {
        if (dead) return;
        canMove = true;
        // Take tick damage
    }

    public abstract void Move(Tile destination);

    public void SetMovementRange(int range)
    {
        movementRange = range;
        gridMovement.SetMovementRange(movementRange);
    }

    public virtual void TakeDamage(int amount)
    {
        healthManager.TakeDamage(amount);
        if (healthManager.Health == 0) Die();
    }

    public virtual void Heal(int amount)
    {
        healthManager.Heal(amount);
    }

    public bool IsDead() { return dead; }

    public virtual void Die()
    {
        dead = true;
    }
}
