using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;         // Player to follow
    public Vector3 offset = new Vector3(0, 2f, -4f); // Offset from the player
    public float sensitivity = 5f;   // Mouse sensitivity
    public float distance = 4f;      // Distance from the player
    public float minY = -35f;        // Min vertical angle
    public float maxY = 60f;         // Max vertical angle

    float yaw = 0f;                  // Horizontal rotation
    float pitch = 20f;               // Vertical rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    void LateUpdate()
    {
        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Build rotation and position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 position = target.position + rotation * new Vector3(0, 0, -distance) + Vector3.up * offset.y;

        // Apply to camera
        transform.position = position;
        transform.rotation = rotation;
    }
}