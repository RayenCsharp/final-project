using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField]private float jumpForce = 7f; // Force applied when jumping

    public Animator animator; // Reference to the Animator component for animations
    private bool isMoving; // Flag to check if the player is moving
    private bool isRunning; // Flag to check if the player is running
    private bool stopMoving;

    private bool specialAttackActive = true; // Flag to check if special attack is active
    [SerializeField]private float specialCoolDown = 15f;

    public PowerUpType currentPowerUp = PowerUpType.None;
    private Coroutine powerupCountdown;

    [SerializeField] private GameObject powerUpObject_1;
    [SerializeField] private GameObject powerUpObject_2;
    [SerializeField] private GameObject powerUpObject_3;
    [SerializeField] private GameObject powerUpObject_4;
    [SerializeField] private AudioClip powerUpAudio;

    [SerializeField] private Damageble damageble;
    [SerializeField] private UiManeger uiManeger;

    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource component for playing audio
    [SerializeField] private Collider playerHitBox; // Reference to the player's hitbox collider
    [SerializeField] private float xBoundLeft = -55f;
    [SerializeField] private float xBoundRight = 40f;
    [SerializeField] private float zBoundBottom = -78f;
    [SerializeField] private float zBoundTop = 40f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody2D component attached to the player
        animator = GetComponentInChildren<Animator>();
        damageble = GetComponentInChildren<Damageble>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the player
        uiManeger.SpecialAttackReady(); // Initialize the UI to show that the special attack is ready
    }

    // Update is called once per frame
    void Update()
    {
        if (!damageble.IsAlive)
        {
            GameManeger gameManeger = Object.FindFirstObjectByType<GameManeger>();
            if (gameManeger != null)
            {
                gameManeger.Invoke("GameOver", 2f);
            }
        }
        if (currentPowerUp == PowerUpType.DoubleSpeed)
        {
            runningSpeed = 15f; // Increase running speed when double speed power-up is active
            if (transform.position.x < xBoundLeft)
            {
                transform.position = new Vector3(xBoundLeft, transform.position.y, transform.position.z);
            }
            if (transform.position.x > xBoundRight)
            {
                transform.position = new Vector3(xBoundRight, transform.position.y, transform.position.z);
            }
            if (transform.position.z < zBoundBottom)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, zBoundBottom);
            }
            if (transform.position.z > zBoundTop)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, zBoundTop);
            }
        }
        else
        {
            runningSpeed = 5f; // Reset to normal running speed
        }

        if (currentPowerUp == PowerUpType.Immunity)
        {
            playerHitBox.enabled = false; // Disable hitbox collider when immunity is active
        }
        else
        {
            playerHitBox.enabled = true; // Enable hitbox collider when immunity is not active
        }

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
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            SpecialAttack();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack_1");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Attack_2");
        }
    }

    void SpecialAttack()
    {
        if (specialAttackActive)
        {
            animator.SetTrigger("Attack_3");
            specialAttackActive = false; // Disable special attack after use
            uiManeger.SpecialAttackNotReady(); // Update UI to indicate special attack is not ready
            StartCoroutine(ResetSpecialAttack(specialCoolDown)); // Start cooldown coroutine
        }
    }

    IEnumerator ResetSpecialAttack (float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        specialAttackActive = true; // Re-enable special attack after cooldown
        uiManeger.SpecialAttackReady(); // Update UI to indicate special attack is ready
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            audioSource.PlayOneShot(powerUpAudio); // Play power-up sound effect
            Destroy(other.gameObject);
            if (currentPowerUp == PowerUpType.Heal)
            {
                damageble.CurrentHealth = damageble.MaxHealth;
                powerUpObject_1.gameObject.SetActive(true);
                powerUpObject_2.gameObject.SetActive(false);
                powerUpObject_3.gameObject.SetActive(false);
                powerUpObject_4.gameObject.SetActive(false);
                uiManeger.ShowHeal(); // Show heal icon in UI
                if (powerupCountdown != null)
                {
                    StopCoroutine(powerupCountdown);
                }
                powerupCountdown = StartCoroutine(PowerupCountdownRoutine(2f , powerUpObject_1));
                Debug.Log("Healed!");
            }
            else if (currentPowerUp == PowerUpType.DoubleDamage)
            {
                powerUpObject_2.gameObject.SetActive(true);
                powerUpObject_1.gameObject.SetActive(false);
                powerUpObject_3.gameObject.SetActive(false);
                powerUpObject_4.gameObject.SetActive(false);
                uiManeger.ActivateDoubleDamage(10f); // Show double damage icon in UI
                if (powerupCountdown != null)
                {
                    StopCoroutine(powerupCountdown);
                }
                powerupCountdown = StartCoroutine(PowerupCountdownRoutine(10f, powerUpObject_2));
                Debug.Log("Double Damage Activated!");
            }
            else if (currentPowerUp == PowerUpType.DoubleSpeed)
            {
                powerUpObject_3.gameObject.SetActive(true);
                powerUpObject_2.gameObject.SetActive(false);
                powerUpObject_1.gameObject.SetActive(false);
                powerUpObject_4.gameObject.SetActive(false);
                uiManeger.ActivateSpeed(10f); // Show double speed icon in UI
                if (powerupCountdown != null)
                {
                    StopCoroutine(powerupCountdown);
                }
                powerupCountdown = StartCoroutine(PowerupCountdownRoutine(10f, powerUpObject_3));
                Debug.Log("Double Speed Activated!");
            }
            else if (currentPowerUp == PowerUpType.Immunity)
            {
                powerUpObject_4.gameObject.SetActive(true);
                powerUpObject_2.gameObject.SetActive(false);
                powerUpObject_3.gameObject.SetActive(false);
                powerUpObject_1.gameObject.SetActive(false);
                uiManeger.ActivateImmunity(5f); // Show immunity icon in UI
                if (powerupCountdown != null)
                {
                    StopCoroutine(powerupCountdown);
                }
                powerupCountdown = StartCoroutine(PowerupCountdownRoutine(5f, powerUpObject_4));
                Debug.Log("Immunity Activated!");
            }
        }
    }

    IEnumerator PowerupCountdownRoutine(float coolDown, GameObject powerUpObject)
    {
        yield return new WaitForSeconds(coolDown);
        currentPowerUp = PowerUpType.None;
        powerUpObject.gameObject.SetActive(false);
        Debug.Log("Power-up expired!");
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
