using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float runningSpeed = 5f; // Speed of the player movement
    [SerializeField] private float walkingSpeed = 2f; // Speed of the player when walking
    private Rigidbody rb; // Reference to the Rigidbody2D component
    private Vector3 movement; // Movement vector
    private float horizontalInput; // Input for horizontal movement
    private float verticalInput; // Input for vertical movement
    public Transform playerCamera; // Reference to the player's camera
    [SerializeField]private bool isGrounded; // Check if the player is grounded
    [SerializeField]private float jumpForce = 5f; // Force applied when jumping

    public Animator animator; // Reference to the Animator component for animations
    bool isMoving; // Flag to check if the player is moving
    bool isRunning; // Flag to check if the player is running
    bool stopMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody2D component attached to the player
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopMoving)
        {
            return;
        }
        AttackManeger();
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        isMoving = horizontalInput != 0 || verticalInput != 0;
        animator.SetBool("isMoving", isMoving);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("isRunning", isRunning);

        Jump();
    }
    void FixedUpdate()
    {
        if (stopMoving)
        {
            return;
        }
        float speed = isRunning ? runningSpeed : walkingSpeed;
        PlayerMovement(horizontalInput, verticalInput, speed);
    }

    void PlayerMovement(float hInput, float vInput, float speed)
    {
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * vInput + camRight * hInput).normalized;

        if (movement.sqrMagnitude > 0.01f)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

            // Smooth rotation
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRotation);
        }
    }



    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump")) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
        }
    }

    void AttackManeger()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack_1");
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Attack_2");
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

    public void FreezeMovement()
    {
        stopMoving = true;
        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void UnfreezeMovement()
    {
        stopMoving = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
