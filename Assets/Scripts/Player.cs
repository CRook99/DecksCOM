using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    HealthBarUI _healthBarUI;

    void Awake()
    {
        healthManager = new HealthManager(100);
        _healthBarUI = TeamUIManager.Instance.CreateHealthBar(healthManager);

        gridMovement = GetComponent<PlayerMovement>();
        movementRange = 9;
        SetMovementRange(movementRange);
    }

    void Update()
    {
        if (Input.GetKeyDown("x")) TakeDamage(10);
        if (Input.GetKeyDown("h")) Heal(10);
    }

    public override void Move(Tile destination)
    {
        // Tick damage
        canMove = false;
        StartCoroutine(gridMovement.MoveToDestination(destination));
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        _healthBarUI.UpdateValues();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        _healthBarUI.UpdateValues();
    }

    public void SetActive()
    {
        gridMovement.CalculateSelectableTiles();
        if (canMove) gridMovement.ShowRange();
    }

    public void SetInactive()
    {
        gridMovement.HideRange();
        gridMovement.ResetAllTiles();
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("Player Die");
    }
}
