using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;

    private float movementSpeed = 12f;
    private float rotationSpeed = 135f;
    private int edgePanMargin = 50;
    [SerializeField] bool useEdgePan = true;
    private bool isKeyPanning = false;

    private Vector3 followOffset;
    private float zoomMin = 5f;
    private float zoomMax = 25f;
    private float zoomSpeed = 10f;

    private void Awake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        Handle();
        // HandleKeyPanInput();
        // HandleRotationInput();
        // HandleZoomInput();
        // if (useEdgePan && !isKeyPanning) { HandleEdgePanInput(); }
        // isKeyPanning = false;
    }

    public void Handle()
    {
        HandleRotationInput();

        if (GameState.Instance.Turn == Turn.ENEMY) return;

        HandleKeyPanInput();
        HandleZoomInput();
        if (useEdgePan && !isKeyPanning) { HandleEdgePanInput(); }
        isKeyPanning = false;
    }

    void HandleKeyPanInput()
    {
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { inputDirection += transform.forward; isKeyPanning = true; }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { inputDirection -= transform.forward; isKeyPanning = true; }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { inputDirection -= transform.right; isKeyPanning = true; }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { inputDirection += transform.right; isKeyPanning = true; }

        transform.position += inputDirection * movementSpeed * GetSpeedModifier() * Time.deltaTime;
    }

    void HandleEdgePanInput()
    {
        Vector3 inputDirection = Vector3.zero;

        if (Input.mousePosition.y > Screen.height - edgePanMargin) { inputDirection += transform.forward; }
        if (Input.mousePosition.y < edgePanMargin) { inputDirection -= transform.forward; }
        if (Input.mousePosition.x < edgePanMargin) { inputDirection -= transform.right; }
        if (Input.mousePosition.x > Screen.width - edgePanMargin) { inputDirection += transform.right; }

        transform.position += inputDirection * movementSpeed * GetSpeedModifier() * Time.deltaTime;
    }

    float GetSpeedModifier()
    {
        if (Input.GetKey(KeyCode.LeftShift)) { return 2f; }
        else if (Input.GetKey(KeyCode.LeftControl)) { return 0.5f; }
        else return 1f;
    }

    void HandleRotationInput()
    {
        float rotateDirection = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Delete)) { rotateDirection += 1f; }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.PageDown)) { rotateDirection -= 1f; }

        transform.eulerAngles += new Vector3(0f, rotateDirection * rotationSpeed * Time.deltaTime, 0f);
    }

    void HandleZoomInput()
    {
        Vector3 zoomDirection = followOffset.normalized;
        if (Input.mouseScrollDelta.y < 0) { followOffset += zoomDirection; }
        if (Input.mouseScrollDelta.y > 0) { followOffset -= zoomDirection; }

        if (followOffset.magnitude < zoomMin) { followOffset = zoomDirection * zoomMin; }
        if (followOffset.magnitude > zoomMax) { followOffset = zoomDirection * zoomMax; }

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, zoomSpeed * Time.deltaTime);
    }

}
