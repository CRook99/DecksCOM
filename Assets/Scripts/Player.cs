using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
[RequireComponent(typeof(PlayerDash))]
public class Player : Character
{
    HealthBarUI _healthBarUI;
    public PlayerDash PlayerDash; // IMPROVE - In composition refactor of Character.cs

    void Awake()
    {
        HealthManager = new HealthManager(100);
        _healthBarUI = TeamUIManager.Instance.CreateHealthBar(HealthManager);

        GridMovement = GetComponent<GridMovement>();
        PlayerDash = GetComponent<PlayerDash>();
        _movementRange = 9;
        SetMovementRange(_movementRange);
    }

    void Update()
    {
        if (Input.GetKeyDown("x")) TakeDamage(10);
        if (Input.GetKeyDown("h")) Heal(10);
    }

    public override void BeginTurn()
    {
        base.BeginTurn();
        PlayerDash.ResetBonus();
        GridMovement.HideRange();
        GridMovement.ResetAllTiles();
    }

    public override void Move(Tile destination)
    {
        // Tick damage
        _canMove = false;
        StartCoroutine(GridMovement.MoveToDestination(destination));
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
        GridMovement.CalculateSelectableTiles();
        if (_canMove) GridMovement.ShowRange();
    }

    public void SetInactive()
    {
        GridMovement.HideRange();
        GridMovement.ResetAllTiles();
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("Player Die");
    }
}
