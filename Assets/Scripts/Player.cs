using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Start()
    {
        gridMovement = GetComponent<PlayerMovement>();
        healthManager = new HealthManager(100);
        movementRange = 9;
        SetMovementRange(movementRange);
    }

    public override void Move(Tile destination)
    {
        // Tick damage
        canMove = false;
        StartCoroutine(gridMovement.MoveToDestination(destination));
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
