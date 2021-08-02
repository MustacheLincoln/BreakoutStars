using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform camFocus;
    private InputMaster inputMaster;
    private Camera cam;
    private float camHeading = -45;
    private float camZoom = 20;
    private float rotateSpeed = 50;
    private float zoomSpeed = 10;
    private Vector2 moveInput;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        inputMaster = new InputMaster();
    }

    private void OnEnable()
    {
        inputMaster.Camera.CameraMovement.Enable();
        inputMaster.Camera.CameraMovement.performed += CaptureMovement;
    }

    private void CaptureMovement(InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        camHeading += moveInput.x * Time.deltaTime * rotateSpeed;
        camZoom += moveInput.y * Time.deltaTime * zoomSpeed;
        camZoom = Mathf.Clamp(camZoom, 10, 40);
        float tilt = camZoom * 2;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(tilt, camHeading, 0), Time.deltaTime * 10);
        transform.position = Vector3.Lerp(transform.position, camFocus.position - transform.forward * camZoom + Vector3.up, Time.deltaTime * 10);
    }
}
