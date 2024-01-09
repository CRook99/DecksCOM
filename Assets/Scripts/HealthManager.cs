using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager
{
    private int _health;
    public int Health { get { return _health; } }
    private int _maxHealth;

    public HealthManager(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }


    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0) { _health = 0; }
    }

    /// <summary>
    /// Applies healing to the character.
    /// </summary>
    /// <param name="amount">Amount of healing to be applied.</param>
    public void Heal(int amount)
    {
        _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
    }

    public float NormalizedHealth()
    {
        return (float) _health / (float) _maxHealth;
    }
}
