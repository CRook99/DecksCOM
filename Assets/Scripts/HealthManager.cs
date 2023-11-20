using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager
{
    private int health;
    private int maxHealth;

    public HealthManager(int pMaxHealth)
    {
        maxHealth = pMaxHealth;
        health = maxHealth;
    }

    /// <summary>
    /// Applies damage to the character. Returns false if the character dies as a result of the damage.
    /// </summary>
    /// <param name="amount">Amount of damage to be applied.</param>
    /// <returns></returns>
    public bool TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Applies healing to the character.
    /// </summary>
    /// <param name="amount">Amount of healing to be applied.</param>
    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public int GetHealth()
    {
        return health;
    }
}
