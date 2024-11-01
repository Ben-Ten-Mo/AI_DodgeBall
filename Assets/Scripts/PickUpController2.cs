using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PickUpController2 : MonoBehaviour
{

    public Rigidbody rb;
    public SphereCollider coll;
    [SerializeField] public Transform player;
    [SerializeField] public Transform ballContainer;
    

    [SerializeField] public float pickUpRange;
    [SerializeField] public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private Vector3 distanceToPlayer;



    public InputActionAsset inputActionAsset; // Reference to your Input Action Asset
    private InputAction pickUpAction;
    private InputAction throwAction;

    private void Start() {
        if (!equipped) {
            rb.isKinematic = false;
            coll.isTrigger = false;
        } else {
            rb.isKinematic = true;
            coll.isTrigger = true;
        }
    }

    private void Update() {
       distanceToPlayer = player.position - transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Make this ignore self collision 
        /*
        if(collision.gameObject.GetInstanceID() != player.gameObject.GetInstanceID()) {
            Debug.Log("self_collision");
            return;
        }
        Debug.Log("hit wall");
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        */
    }


    private void OnEnable()
    {
        Debug.Log("enable called!");

        // Find and enable actions
        var playerActionMap = inputActionAsset.FindActionMap("Player"); // Replace with your action map name

        throwAction = playerActionMap.FindAction("Throw"); // Replace with your action name
        pickUpAction = playerActionMap.FindAction("Pick_Up"); // Replace with your action name

        throwAction.Enable();
        pickUpAction.Enable();

        // Subscribe to the action events
        throwAction.performed += OnThrow;
        pickUpAction.performed += OnPickUp;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable actions
        throwAction.performed -= OnThrow;
        pickUpAction.performed -= OnPickUp;

        throwAction.Disable();
        pickUpAction.Disable();
    }

    private void OnPickUp(InputAction.CallbackContext context)
    {
        Debug.Log("Pickup action called!");

        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && !slotFull) {
            equipped = true;
            slotFull = true;

            transform.SetParent(ballContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            rb.isKinematic = true;
            coll.enabled = false;
        }
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        Debug.Log("Throw action called!"); // Log when jump is triggered

        if (!equipped) return;

        equipped = false;
        slotFull = false;

        transform.SetParent(null);
        
        rb.isKinematic = false;
        rb.useGravity = false;
        coll.enabled = true;

        // Debug.Log(player.forward * dropForwardForce);
        rb.AddForce(player.forward * dropForwardForce, ForceMode.Impulse);
    }

}
