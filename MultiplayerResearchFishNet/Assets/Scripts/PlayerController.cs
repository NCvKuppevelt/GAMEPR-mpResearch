using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Connection;
using FishNet.Object;

// Mostly based on https://www.youtube.com/watch?v=oUPlKYlb27Y
public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 10f;
    private float movementFrontBack;
    private float movementRightLeft;

    private float cameraYOffset = .4f;
    private Camera playerCamera;

    // All multiplayer code is in this function
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.transform.position = new Vector3(
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
    }

    // void OnMove(InputValue movementValue)
    // {
    //     var movementVector = movementValue.Get<Vector2>();
    //     movementFrontBack = movementVector.y;
    //     movementRightLeft = movementVector.x;
    // }

    private void FixedUpdate()
    {
        // MoveCharacter();
    }

    // Method for moving with PlayerInput component
    private void MoveCharacter()
    {
        transform.position += 
            transform.forward * (moveSpeed * movementFrontBack * Time.deltaTime) +
            transform.right * (moveSpeed * movementRightLeft * Time.deltaTime);
    }
}
