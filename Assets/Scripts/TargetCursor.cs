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

    GameObject targetObject;

    private bool locked = false;

    private void FixedUpdate()
    {
        gizmo.transform.position = new Vector3(transform.localPosition.x, (0.25f * Mathf.Sin(Time.time * 2)) + transform.localPosition.y + 1f, transform.localPosition.z);
        gizmo.transform.Rotate(gizmoRotation);
    }

    private void Update()
    {
        ProcessSelection(TileSelection.instance.current);
        CheckLockInput();
    }

    void ProcessSelection(GameObject target)
    {
        if (target.tag == "Tile" && !locked)
        {
            transform.position = target.transform.position + offset;
            if (target.GetComponent<Tile>().Occupied())
            {
                UpdateColour(invalidMaterial);
            }
            else
            {
                UpdateColour(validMaterial);
            }
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

    // Will be replaced when actual cursor modelled
    void UpdateColour(Material material)
    {
        gizmo.GetComponent<MeshRenderer>().material = material;
        foreach (Transform child in marker.transform)
        {
            child.GetComponent<MeshRenderer>().material = material;
        }
    }
}
