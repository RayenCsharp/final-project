using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        idle, // Enemy is idle
        Roaming, // Enemy is roaming
        Chasing, // Enemy is chasing the player
        attacking // Enemy is attacking the player
    }
    [SerializeField]private State currentState; // Current state of the enemy


    [SerializeField] private float walkingSpeed = 2.5f; 
    [SerializeField] private float runningSpeed = 4.5f; 
    private Rigidbody rb; // Rigidbody component of the enemy

    private Vector3 startingPosition; // Starting position of the enemy
    private Vector3 roamingPostion; // Roaming position of the enemy
    private Vector3 chasingPostion; // Chasing position of the enemy

    private float roamingRange = 10f; // Range within which the enemy can roam
    private float waitTime;
    private float waitTimer; // Timer to track roaming time
    [SerializeField]private bool enemystoped; // Flag to check if the enemy is roaming
    private bool isAttacking = false;

    public DetectionZone obstacleDetectionZone; // Reference to the detection zone for obstacles
    public DetectionZone targetingZone; // Reference to the detection zone for targeting
    public DetectionZone attackDetectionZone; // Reference to the detection zone for attack detection

    private Animator Animator;
    private AudioSource AudioSource;

    [SerializeField]private AudioClip walkingSound;
    [SerializeField]private AudioClip runningSound;
    [SerializeField]private AudioClip swordSwingSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Roaming; // Set the initial state to Roaming
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        startingPosition = transform.position; // Set the starting position to the enemy's current position
        SetNewRoamingPosition(); // Set the initial roaming position
    }

    // Update is called once per frame
    void Update()
    {
        DetermineState();
        HandlingSounds();

        Animator.SetBool("isMoving", currentState == State.Chasing || currentState == State.Roaming);
        Animator.SetBool("isChasing", currentState == State.Chasing);
        Animator.SetBool("isAttacking", currentState == State.attacking);

        switch (currentState)
        {
            case State.Roaming:
                
                if (Vector3.Distance(transform.position, roamingPostion) < 0.1f)
                {
                    if (!enemystoped)
                        StartWaiting(1f); // Reached destination
                }
                else if (obstacleDetectionZone.detectedColliders.Count > 0)
                {
                    if (!enemystoped)
                        SetNewRoamingPosition();
                }
                break;

            case State.idle:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    enemystoped = false;
                    waitTimer = 0f;
                    SetNewRoamingPosition();
                }
                break;

            case State.Chasing:
                if (targetingZone.detectedColliders.Count > 0)
                {
                    Transform target = targetingZone.detectedColliders[0].transform;
                    chasingPostion = (target.position - transform.position).normalized;
                }
                break;

            case State.attacking:
                if (!isAttacking)
                {
                    isAttacking = true;
                    Invoke(nameof(EndAttack), 2.25f); // duration of attack animation
                }
                break;
        }

    }

    void EndAttack()
    {
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (enemystoped) return;
        if (!enemystoped && currentState == State.Roaming) // Check if the enemy is not stopped
        {
            Vector3 direction = (roamingPostion - transform.position).normalized; // Calculate the direction towards the roaming position
            rb.MovePosition(transform.position + direction * walkingSpeed * Time.fixedDeltaTime); // Move the enemy towards the roaming position
            if (direction != Vector3.zero) // Check if the direction is not zero to avoid unnecessary rotation
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate the rotation towards the direction
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * walkingSpeed)); // Smoothly rotate the enemy towards the direction
            }
        }
        else if (currentState == State.Chasing) // Check if the enemy is in the Chasing state and there are detected colliders in the targeting zone
        {
            rb.MovePosition(transform.position + chasingPostion * runningSpeed * Time.fixedDeltaTime); // Move the enemy towards the target
            if (chasingPostion.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(chasingPostion);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * runningSpeed));
            }

        }
    }

    void SetNewRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized; // Generate a random direction
        roamingPostion = startingPosition + randomDirection * Random.Range(0f, roamingRange); // Set the new roaming position based on the random direction and range
    }

    void StartWaiting(float newWaitTime)
    {
        currentState = State.idle;
        enemystoped = true;
        waitTime = newWaitTime;
        waitTimer = 0f;
    }

    private void DetermineState()
    {
        if (isAttacking)
        {
            currentState = State.attacking;
        }
        else if (attackDetectionZone.detectedColliders.Count > 0)
        {
            currentState = State.attacking;
        }
        else if (targetingZone.detectedColliders.Count > 0)
        {
            currentState = State.Chasing;
        }
        else if (!enemystoped)
        {
            currentState = State.Roaming;
        }
        else
        {
            currentState = State.idle;
        }
    }

    void HandlingSounds()
    {
        switch (currentState)
        {
            case State.Roaming:
                if (!AudioSource.isPlaying || AudioSource.clip != walkingSound)
                {
                    AudioSource.clip = walkingSound;
                    AudioSource.volume = 0.5f;
                    AudioSource.loop = true;
                    AudioSource.Play();
                }
                break;
            case State.Chasing:
                if (!AudioSource.isPlaying || AudioSource.clip != runningSound)
                {
                    AudioSource.clip = runningSound;
                    AudioSource.volume = 0.3f;
                    AudioSource.loop = true;
                    AudioSource.Play();
                }
                break;
            case State.attacking:
                if (!AudioSource.isPlaying || AudioSource.clip != swordSwingSound)
                {
                    AudioSource.clip = swordSwingSound;
                    AudioSource.loop = false;
                    AudioSource.Play();
                }
                break;
            default:
                if (AudioSource.isPlaying)
                {
                    AudioSource.Stop();
                    AudioSource.clip = null;
                } 
                break;
        }
    }

    

    public void FreezeMovement()
    {
        enemystoped = true;
        currentState = State.idle; // Set the state to idle when frozen
        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void UnfreezeMovement()
    {
        enemystoped = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Freeze position on X and Y axes and rotation on Z axis

    }

}
