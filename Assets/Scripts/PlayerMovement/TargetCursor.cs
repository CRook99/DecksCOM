using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TargetCursor : MonoBehaviour, IPlayerMovement
{
    public GameObject marker;
    public GameObject gizmo;
    Vector3 offset = new (0f, 0.5f, 0f);
    [SerializeField] float _cycleLength;
    [SerializeField] float _height;
    [SerializeField] float _rotationDuration;
    

    // Will be replaced when actual cursor modelled
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    Material currentMaterial;
    bool targetIsOccupied;

    GameObject targetObject;

    bool locked;

    void Start()
    {
        gizmo.transform.DOLocalMoveY(_height, _cycleLength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        gizmo.transform.DORotate(new Vector3(0, 360, 0), _rotationDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    void Update()
    {
        ProcessSelection(TileSelection.Instance.Current);
        //CheckLockInput();
    }

    void ProcessSelection(GameObject target)
    {
        if (target.CompareTag("Tile") && !locked)
        {
            transform.position = target.transform.position + offset;
            targetIsOccupied = target.GetComponent<Tile>().Occupied();
            UpdateAppearance();
        }
    }

    void CheckLockInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            Lock();
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Unlock();
        }
    }

    public GameObject GetTarget()
    {
        return targetObject;
    }

    void Lock()
    {
        locked = true;
    }

    void Unlock()
    {
        locked = false;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    // Updates cursor appearance based on if the target tile is walkable
    // Will be replaced when actual cursor modelled
    void UpdateAppearance()
    {
        gizmo.GetComponent<MeshRenderer>().enabled = !targetIsOccupied;
        currentMaterial = targetIsOccupied ? invalidMaterial : validMaterial;

        foreach (Transform child in marker.transform)
        {
            child.GetComponent<MeshRenderer>().material = currentMaterial;
        }
    }
}
