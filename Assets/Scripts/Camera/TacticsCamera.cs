using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsCamera : MonoBehaviour
{
    [SerializeField] new Camera camera;

    private Vector3 _position;
    private float _movementSpeed;
    private float _movementTime;
    private float _movementSpeedModifier;
    
    private Quaternion _rotation;
    private float _rotationSpeed;

    private float _zoomSpeed;
    private float _maxZoom = -4f;
    private float _minZoom = -11f;


    

    void Start()
    {
        camera.transform.eulerAngles = new Vector3(45f, 45f, 0f);
        _position = transform.position;
        _rotation = transform.rotation;
        _movementSpeed = 0.05f;
        _movementSpeedModifier = 1f;
        _movementTime = 10f;
        _rotationSpeed = 0.5f;

        _zoomSpeed = 1f;
    }

    private void Update()
    {
        HandlePanInput();
        HandleSpeedInput();
        HandleZoomInput();
        HandleRotationInput();
        ClampZoom();
    }

    void HandlePanInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { _position += (transform.forward + transform.right) * _movementSpeed * _movementSpeedModifier; }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { _position += (-transform.forward + -transform.right) * _movementSpeed * _movementSpeedModifier; }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { _position += (transform.forward + -transform.right) * _movementSpeed * _movementSpeedModifier; }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { _position += (-transform.forward + transform.right) * _movementSpeed * _movementSpeedModifier; }

        transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime * _movementTime);
    }

    void HandleSpeedInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) { _movementSpeedModifier = 2f; }
        else if (Input.GetKey(KeyCode.LeftControl)) { _movementSpeedModifier = 0.5f; }
        else _movementSpeedModifier = 1f;
    }

    void HandleRotationInput()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Delete)) { _rotation *= Quaternion.Euler(Vector3.up * _rotationSpeed); }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.PageDown)) { _rotation *= Quaternion.Euler(Vector3.up * -_rotationSpeed); }

        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.deltaTime * _movementTime);
    }

    void HandleZoomInput()
    {
        if (Input.GetAxis("Scroll") > 0 && !CameraTooClose()) { camera.transform.position -= -camera.transform.forward * _zoomSpeed; }
        if (Input.GetAxis("Scroll") < 0 && !CameraTooFar()) { camera.transform.position += -camera.transform.forward * _zoomSpeed; }
    }

    void ClampZoom()
    {
        if (CameraTooClose())
        {
            camera.transform.localPosition = new Vector3(_maxZoom, camera.transform.localPosition.y, _maxZoom);
        }

        if (CameraTooFar())
        {
            camera.transform.localPosition = new Vector3(_minZoom, camera.transform.localPosition.y, _minZoom);
        }
    }

    bool CameraTooClose() { return camera.transform.localPosition.x >= _maxZoom; }

    bool CameraTooFar() { return camera.transform.localPosition.x <= _minZoom; }
}
