using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    public GameObject marker;
    public GameObject gizmo;
    private Vector3 gizmoRotation = new Vector3(0f, 1f, 0f);
    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    // Will be replaced when actual cursor modelled
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    private Material currentMaterial;
    private bool targetIsOccupied;

    GameObject targetObject;

    private bool locked = false;

    private void FixedUpdate()
    {
        gizmo.transform.position = new Vector3(transform.localPosition.x, (0.25f * Mathf.Sin(Time.time * 2)) + transform.localPosition.y + 1f, transform.localPosition.z);
        gizmo.transform.Rotate(gizmoRotation);
    }

    private void Update()
    {
        ProcessSelection(TileSelection.Instance.Current);
        //CheckLockInput();
    }

    void ProcessSelection(GameObject target)
    {
        if (target.tag == "Tile" && !locked)
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
