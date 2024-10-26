using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class PickUpController : MonoBehaviour
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

    public void PickUp(InputAction.CallbackContext context) {
        if (!context.started) return;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && !slotFull) {
            equipped = true;
            slotFull = true;

            transform.SetParent(ballContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            rb.isKinematic = true;
            coll.isTrigger = true;
        }
    }

    public void Drop(InputAction.CallbackContext context) {
        if (!context.started || !equipped) return;
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        //rb.linearVelocity = player.GetComponent.linearVelocity;

        //rb.AddForce(camera.forward * dropForwardForce, ForceMode.Impulse);
        //rb.AddForce(camera.up * dropUpwardForce, ForceMode.Impulse);
        
        rb.isKinematic = false;
        coll.isTrigger = false;
    }

    public void Throw(InputAction.CallbackContext context) {
        if (!context.started || !equipped) return;
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        //rb.linearVelocity = player.GetComponent.linearVelocity;

        //rb.AddForce(camera.forward * dropForwardForce, ForceMode.Impulse);
        //rb.AddForce(camera.up * dropUpwardForce, ForceMode.Impulse);
        
        rb.isKinematic = false;
        coll.isTrigger = false;

        //transform.position += player.right * 40.0f * Time.deltaTime;
        //rb.Move(player.right * 40.0f * Time.deltaTime);
    }

}
