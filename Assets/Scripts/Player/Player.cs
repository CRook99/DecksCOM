using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(HealthManager))]
public class Player : MonoBehaviour
{
    // References
    HealthManager _healthManager;
    GridMovement _gridMovement;
    HealthBarUI _healthBarUI;
    PlayerDash _playerDash;
    
    // Stats
    public bool Dead { get; private set; }
    public bool CanMove { get; private set; }

    void Awake()
    {
        _healthManager = GetComponent<HealthManager>();
        _gridMovement = GetComponent<GridMovement>();
        _playerDash = GetComponent<PlayerDash>();
    }

    void Start()
    {
        _healthBarUI = TeamUIManager.Instance.CreateHealthBar(_healthManager);
        TeamManager.Instance.AddPlayer(this);
    }

    void OnEnable()
    {
        GameState.OnBeginPlayerTurn += BeginTurn;
    }

    void OnDisable()
    {
        GameState.OnBeginPlayerTurn -= BeginTurn;
    }

    void Update()
    {
        if (Input.GetKeyDown("x")) TakeDamage(10);
        if (Input.GetKeyDown("h")) Heal(10);
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

    public Tile GetCurrentTile()
    {
        return _gridMovement.GetCurrentTile();
    }

    public void Heal(int amount)
    {
        _healthManager.Heal(amount);
        _healthBarUI.UpdateValues();
    }

    public void TakeDamage(int amount)
    {
        _healthManager.TakeDamage(amount);
        if (_healthManager.Health == 0) Die();
        
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

    public void Die()
    {
        Dead = true;
        Debug.Log("Player Die");
    }
}
