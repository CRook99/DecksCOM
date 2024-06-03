using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDash))]
public class Player : Character
{
    // References
    HealthBarUI _healthBarUI;
    PlayerDash _playerDash;

    protected override void Awake()
    {
        base.Awake();
        _playerDash = GetComponent<PlayerDash>();
    }

    protected override void Start()
    {
        base.Start();
        _healthBarUI = TeamUIManager.Instance.CreateHealthBar(_healthManager);
        TeamManager.Instance.AddPlayer(this);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown("x")) Damage(10);
        if (Input.GetKeyDown("h")) Heal(10);
    }
    
    void OnEnable()
    {
        GameState.OnBeginPlayerTurn += BeginTurn;
    }

    void OnDisable()
    {
        GameState.OnBeginPlayerTurn -= BeginTurn;
    }

    public void BeginTurn()
    {
        if (Dead) return;
        CanMove = true;
        
        _playerDash.ResetBonus();
        _gridMovement.HideRange();
        _gridMovement.CalculateSelectableTiles();
    }

    public void Move(Tile destination)
    {
        // Tick damage
        CanMove = false;
        StartCoroutine(_gridMovement.MoveToDestination(destination));
    }

    public List<Tile> GetMovementArea()
    {
        return _gridMovement.GetReachableTiles();
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        _healthBarUI.UpdateValues();
    }

    public override void Damage(int amount)
    {
        base.Damage(amount);
        _healthBarUI.UpdateValues();
    }

    public void SetActive()
    {
        _gridMovement.CalculateSelectableTiles();
        if (CanMove) _gridMovement.ShowRange();
    }

    public void SetInactive()
    {
        _gridMovement.HideRange();
        _gridMovement.ResetAllTiles();
    }

    public override void Die()
    {
        Debug.Log("Player Die");
    }
}
