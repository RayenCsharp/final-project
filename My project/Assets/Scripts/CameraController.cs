using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("follow player settings")]
    public Transform target; // The target to follow
    [SerializeField] private float smoothSpeed = 0.25f; // Speed of the camera movement
    [SerializeField] private Vector3 offset = new Vector3 (0, 0.8f, 0); // Offset from the target position

    [Header("Camera Rotation Sittingq")]
    public float cameraSentetivity = 500f; // Sensitivity of the camera rotation
    private float xMouseInput; // Input for the X axis (mouse movement)
    private float yMouseInput; // Input for the Y axis (mouse movement)
    private float xRotation = 0f; // Rotation around the X axis
    private float yRotation = 0f; // Rotation around the Y axis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    void LateUpdate()
    {
        FollowPlayer();
    }


    void FollowPlayer()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPostion;
    }

    void RotateCamera()
    {
        xMouseInput = Input.GetAxis("Mouse X") * cameraSentetivity * Time.deltaTime; // Get mouse input for X axis
        yMouseInput = Input.GetAxis("Mouse Y") * cameraSentetivity * Time.deltaTime; // Get mouse input for Y axis
        xRotation -= yMouseInput; // Update X rotation based on mouse input
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp X rotation to prevent flipping
        yRotation += xMouseInput; // Update Y rotation based on mouse input
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f); // Apply the rotation to the camera
    }
}
