using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamSwitcher : MonoBehaviour
{
    static TeamSwitcher _instance;
    public static TeamSwitcher Instance { get { return _instance; } }
    [SerializeField] int _currentIndex;
    bool canSwitch = true;

    public static event Action<int> OnSwitch;

    void Awake()
    {
        _instance = this;
        _currentIndex = 0;
    }

    void OnEnable()
    {
        MovementSelection.OnBeginMove += Disable;
        GridMovement.OnEndMove += Enable;
        GameState.OnBeginPlayerTurn += BeginTurn;
    }

    void OnDisable()
    {
        MovementSelection.OnBeginMove -= Disable;
        GridMovement.OnEndMove -= Enable;
    }

    void Update()
    {
        if (!canSwitch) return;

        if (Input.inputString != "")
        {
            bool isNumber = Int32.TryParse(Input.inputString, out var number);
            if (!isNumber || number < 1 || number > 3) return;
            number--;
            
            if (TeamManager.Instance.GetPlayerByIndex(number).Dead) return;
            if (_currentIndex == number) // Player already selected
            {
                StartCoroutine(CameraSystem.Instance.MoveToPoint(TeamManager.Instance.Current.gameObject));
                return;
            }
            
            _currentIndex = number;
            Switch(number);
        }
    }

    void Switch(int index)
    {
        TeamManager.Instance.SetCurrent(index);
        
        OnSwitch?.Invoke(index);
        
        StartCoroutine(CameraSystem.Instance.MoveToPoint(TeamManager.Instance.Current.gameObject));
    }

    void BeginTurn()
    {
        Switch(0);
    }

    public void Enable() { canSwitch = true; }
    public void Disable(GameObject o) { canSwitch = false; }
}
