using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField]
    [Range(1f, 25f)]
    private float moveSpeed;
    
    [Header("Camera")]
    [SerializeField]
    private Transform cameraContainer;

    [SerializeField]
    [Range(-120, -45)] 
    private int minXLook;

    [SerializeField]
    [Range(45, 120)]
    private int maxXLook;
    
    [SerializeField]
    [Range(0.1f, 2f)]
    private float lookSensitivity;

    private Rigidbody playerRigidbody;
    
    private float camCurXRot;
    private Vector2 mouseDelta;
    private Vector2 moveInput;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDir = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        moveDir *= moveSpeed;
        moveDir.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = moveDir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f,0f);
        transform.eulerAngles += new Vector3(0f, mouseDelta.x * lookSensitivity, 0f);
    }
    
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
    }
}
