using System;
using UnityEngine;
using FishNet.Object;
using Cursor = UnityEngine.Cursor;

//This is made by Bobsi Unity - Youtube
//Edited by 2425S2 minor GAMEPR Team Multiplayer
public class PlayerController : NetworkBehaviour
{
    [Header("Base setup")] public float walkingSpeed = 7.5f;
    public                        float runningSpeed = 11.5f;
    public                        float jumpSpeed    = 8.0f;
    public                        float gravity      = 20.0f;
    public                        float lookSpeed    = 2.0f;
    public                        float lookXLimit   = 45.0f;

    private CharacterController characterController;
    private Vector3             moveDirection = Vector3.zero;
    private float               rotationX;

    [HideInInspector] public bool canMove = true;

    [SerializeField] private float  cameraYOffset = 0.6f;
    private                  Camera playerCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera!.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + cameraYOffset,
                transform.position.z
            );
            playerCamera.transform.SetParent(transform);
        }
        else
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Press Left Shift to run
        var isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        var forward = transform.TransformDirection(Vector3.forward);
        var right   = transform.TransformDirection(Vector3.right);

        var curSpeedX          = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        var curSpeedY          = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        var movementDirectionY = moveDirection.y;
        moveDirection = forward * curSpeedX + right * curSpeedY;

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (!canMove || !playerCamera) return;
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}