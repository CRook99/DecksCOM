using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public GridMovement GridMovement;
    protected HealthManager HealthManager;

    // Movement
    protected int _movementRange;
    protected bool _canMove;
    public bool CanMove { get { return _canMove; } }

    // Stats
    protected int maxHealth;
    protected int _atkStacks;
    protected int _defStacks;
    
    protected bool dead;

    public virtual void BeginTurn()
    {
        if (dead) return;
        _canMove = true;
        // Take tick damage
    }

    public abstract void Move(Tile destination);

    public void SetMovementRange(int range)
    {
        _movementRange = range;
        GridMovement.SetMovementRange(_movementRange);
    }

    public virtual void TakeDamage(int amount)
    {
        HealthManager.TakeDamage(amount);
        if (HealthManager.Health == 0) Die();
    }

    public virtual void Heal(int amount)
    {
        HealthManager.Heal(amount);
    }

    public bool IsDead() { return dead; }

    public virtual void Die()
    {
        dead = true;
    }
}
