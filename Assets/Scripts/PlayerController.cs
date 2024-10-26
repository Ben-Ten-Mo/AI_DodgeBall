using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement Variables
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;
    [SerializeField] private float speed;

    //Movement Variables for Sprinting Action
    [SerializeField] private Movement movement;

    //Rotation Variables
    [SerializeField] private float rotationSpeed = 500f;
    private Camera mainCamera;
    

    //Gravity Variables
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float velocity;

    // Jump Variables
    [SerializeField] private float jumpPower;
    private int numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

    //Dash Variables
    [SerializeField] private float maxNumberOfDashes;
    private int numberofDashes;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private bool dashing = false;
    private float lastDashTime;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update() {
        Debug.Log(direction * dashSpeed * Time.deltaTime);
        ApplyRotation();
        if (!dashing) {
            ApplyGravity();
        }
        ApplyMovement();
        
    }

    private void ApplyRotation() {
        if (input.sqrMagnitude == 0) return;

        direction = Quaternion.Euler(0.0f, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(input.x, 0.0f, input.y);
        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement() {
        var targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.aceleration * Time.deltaTime);

        characterController.Move(direction * movement.currentSpeed * Time.deltaTime);
    }

    private void ApplyGravity() {
        if (IsGrounded() && velocity < 0.0f) {
            velocity = -1.0f;
        } else {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        
        direction.y = velocity;
    }

    public void Move(InputAction.CallbackContext context) {

        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0.0f, input.y);

    }

    public void Jump(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (!IsGrounded() && numberOfJumps >= maxNumberOfJumps) return;
        if (numberOfJumps == 0) StartCoroutine(WaitForLanding());
        numberOfJumps++;
        velocity = jumpPower / numberOfJumps;
    }

    public void Sprint(InputAction.CallbackContext context) {

        movement.isSprinting = context.started || context.performed;
    }

    public void Dash(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (numberofDashes >= maxNumberOfDashes || Time.time <= lastDashTime + dashCooldown) return;
        StartCoroutine(Dash());
        numberofDashes++;
    }

    private IEnumerator WaitForLanding() {
        // After the character as left the ground then run to check when it's grounded again
        yield return new WaitUntil(() => !IsGrounded());
        // Constantly checks if the character is grounded
        yield return new WaitUntil(IsGrounded);
        // reset number of jumps
        numberOfJumps = 0;
    }

    private bool IsGrounded() => characterController.isGrounded;

    private IEnumerator Dash() {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime) {
            dashing = true;
            characterController.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
        lastDashTime = Time.time;
        dashing = false;
    }   
}

[System.Serializable]
public struct Movement {
    public float speed;
    public float multiplier;
    public float aceleration;

    // We don't need to see this field to adjust
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public float currentSpeed;
}