using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // Our player
    [SerializeField] private Transform target;
    // How far we want our camera from our player
    private float distanceToPlayer;
    
    // Input from Mouse Delta for the Look Action
    private Vector2 input;
    
    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;
    
    private CameraRotation cameraRotation;

    private void Awake() {
        distanceToPlayer = Vector3.Distance(transform.position, target.position);
    }

    public void Look(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }

    private void Update() {
        cameraRotation.Yaw += input.x * mouseSensitivity.horizontal * BoolToInt(mouseSensitivity.invertHorizontal) * Time.deltaTime;
        cameraRotation.Pitch += input.y * mouseSensitivity.vertical * BoolToInt(mouseSensitivity.invertVertical) * Time.deltaTime;
        // Ensure that the cameraRotation for pitch will always be inbetween our bounds
        cameraRotation.Pitch = Mathf.Clamp(cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);
    }

    private void LateUpdate() {
        transform.eulerAngles = new Vector3(cameraRotation.Pitch, cameraRotation.Yaw, 0.0f);
        transform.position = target.position - transform.forward * distanceToPlayer;
    }

    private static int BoolToInt(bool b) => b ? 1 : -1;
}

[System.Serializable]
public struct MouseSensitivity {
    //Keeps mouse sensitivity x and y together
    public float horizontal;
    public float vertical;
    public bool invertVertical;
    public bool invertHorizontal;
}

public struct CameraRotation {
    //Camera rotation on the X axis
    public float Pitch;
    //Camera rotation on the Y axis
    public float Yaw;
}

[System.Serializable]
public struct CameraAngle {
    //Sets limits to the rotation of the camera
    public float min;
    public float max;
}