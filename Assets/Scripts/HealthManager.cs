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

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public int GetHealth()
    {
        return health;
    }
}
