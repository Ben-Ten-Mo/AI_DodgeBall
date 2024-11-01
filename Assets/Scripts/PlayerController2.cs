using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    public InputActionAsset inputActionAsset; // Reference to your Input Action Asset
    private InputAction moveAction;
    private InputAction jumpAction;

    private float deltaTime;

    private void Update()
    {
        deltaTime = Time.deltaTime;
        // Movement logic in Update
        //Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        //transform.Translate(movement * Time.deltaTime); // Apply movement using deltaTime
    }

    private void OnEnable()
    {
        // Find and enable actions
        var playerActionMap = inputActionAsset.FindActionMap("Player"); // Replace with your action map name

        moveAction = playerActionMap.FindAction("Move"); // Replace with your action name
        jumpAction = playerActionMap.FindAction("Jump"); // Replace with your action name

        moveAction.Enable();
        jumpAction.Enable();

        // Subscribe to the action events
        moveAction.performed += OnMove;
        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable actions
        moveAction.performed -= OnMove;
        jumpAction.performed -= OnJump;

        moveAction.Disable();
        jumpAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>(); // Get the input value
        Debug.Log("Move action called: " + moveInput); // Log the movement

        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y); // Create movement vector

        //transform.Translate(Vector3.forward * Time.deltaTime);

        Debug.Log("Move action called: " + (movement * deltaTime)); // Log the movement

        transform.Translate(movement * deltaTime); // Move the GameObject
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump action called!"); // Log when jump is triggered
    }
}