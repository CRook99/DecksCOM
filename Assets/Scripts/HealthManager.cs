using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    int _health;
    public int Health => _health;
    [SerializeField] int _maxHealth;

    void Awake()
    {
        _health = _maxHealth;
    }


    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0) { _health = 0; }
    }

    
    public void Heal(int amount)
    {
        _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
    }

    
    public float NormalizedHealth()
    {
        return (float) _health / (float) _maxHealth;
    }
}
