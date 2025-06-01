using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 5f; // Speed of the player movement
    private Rigidbody rb; // Reference to the Rigidbody2D component
    private Vector3 movement; // Movement vector
    private float horizontalInput; // Input for horizontal movement
    private float verticalInput; // Input for vertical movement
    public Transform playerCamera; // Reference to the player's camera
    [SerializeField]private bool isGrounded; // Check if the player is grounded
    [SerializeField]private float jumpForce = 5f; // Force applied when jumping

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody2D component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Get horizontal input (A/D keys or Left/Right arrows)
        verticalInput = Input.GetAxis("Vertical"); // Get vertical input (W/S keys or Up/Down arrows)

        Vector3 camFarward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camFarward.y = 0; // Set the y component to 0 to keep movement on the ground plane
        camRight.y = 0; // Set the y component to 0 to keep movement on the ground plane
        camFarward.Normalize(); // Normalize the forward vector to ensure consistent movement speed
        camRight.Normalize(); // Normalize the right vector to ensure consistent movement speed

        movement = (camFarward * verticalInput + camRight * horizontalInput).normalized;
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump")) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded");
        }
    }
}
