using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour
{
    private enum State
    {
        idle, // Enemy is idle
        Roaming, // Enemy is roaming
        Chasing, // Enemy is chasing the player
        normalAttack,
        ChargeAttack // Enemy is attacking the player
    }
    [SerializeField] private State currentState; // Current state of the enemy


    [SerializeField] private float walkingSpeed = 2.5f;
    [SerializeField] private float runningSpeed = 4.5f;
    private Rigidbody rb; // Rigidbody component of the enemy

    private Vector3 startingPosition; // Starting position of the enemy
    private Vector3 roamingPostion; // Roaming position of the enemy
    private Vector3 chasingPostion; // Chasing position of the enemy

    private float roamingRange = 30f; // Range within which the enemy can roam
    private float waitTime;
    private float waitTimer;
    [SerializeField] private bool enemystoped; // Flag to check if the enemy is roaming
    private bool isAttacking = false;

    public DetectionZone obstacleDetectionZone; // Reference to the detection zone for obstacles
    public DetectionZone targetingZone; // Reference to the detection zone for targeting
    public DetectionZone attackDetectionZone; // Reference to the detection zone for attack detection

    private Animator Animator;
    private AudioSource AudioSource;
    private Damageble Damageble; // Reference to the Damageble component


    [SerializeField]private AudioClip walkingSound;
    [SerializeField]private AudioClip runningSound;

    [SerializeField] private AudioClip attackSound1;
    [SerializeField] private AudioClip attackSound2;
    [SerializeField] private AudioClip attackSound3;

    /*
    [SerializeField]private AudioClip attackSound;
    [SerializeField]private AudioClip chargeAttackSound;
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Roaming; // Set the initial state to Roaming
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        Damageble = GetComponent<Damageble>(); // Get the Damageble component
        startingPosition = transform.position; // Set the starting position to the enemy's current position
        SetNewRoamingPosition(); // Set the initial roaming position
    }

    // Update is called once per frame
    void Update()
    {
        DetermineState();
        HandlingSounds();
        
        if (!Damageble.IsAlive) return; // If the enemy is not alive, do not update

        Animator.SetBool("isMoving", currentState == State.Chasing || currentState == State.Roaming);
        Animator.SetBool("isChasing", currentState == State.Chasing);
        Animator.SetBool("isAttacking", currentState == State.normalAttack);

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
                    enemystoped = false;
                    Transform target = targetingZone.detectedColliders[0].transform;
                    chasingPostion = (target.position - transform.position).normalized;
                }
                break;

            case State.normalAttack:
                if (!isAttacking)
                {
                    isAttacking = true;
                    BasicAttacks();
                }
                break;
            case State.ChargeAttack:
                //methode for charge attack
                break;
        }

    }

    void EndAttack()
    {
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (enemystoped || !Damageble.IsAlive) return;
        if (currentState == State.Roaming)
        {
            Vector3 direction = roamingPostion - transform.position;
            direction.y = 0f;

            float distance = direction.magnitude;

            if (distance < 0.5f)
            {
                StartWaiting(1f);
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                return;
            }

            direction.Normalize();
            rb.MovePosition(transform.position + direction * walkingSpeed * Time.fixedDeltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * walkingSpeed));
        }
        else if (currentState == State.Chasing)
        {
            rb.MovePosition(transform.position + chasingPostion * runningSpeed * Time.fixedDeltaTime);
            if (chasingPostion.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(chasingPostion);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * runningSpeed));
            }
        }
    }

    void SetNewRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        roamingPostion = startingPosition + randomDirection * Random.Range(20f, roamingRange);
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
            return;
        }
        else if (attackDetectionZone.detectedColliders.Count > 0)
        {
            currentState = State.normalAttack;
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
        if (!Damageble.IsAlive)
        {
            if (AudioSource.isPlaying) AudioSource.Stop(); return;
        }

        if (currentState == State.normalAttack || currentState == State.ChargeAttack)
            return;
        switch (currentState)
        {
            case State.Roaming:
                if (!AudioSource.isPlaying || AudioSource.clip != walkingSound)
                {
                    AudioSource.clip = walkingSound;
                    AudioSource.volume = 1.5f;
                    AudioSource.loop = true;
                    AudioSource.Play();
                }
                break;

            case State.Chasing:
                if (!AudioSource.isPlaying || AudioSource.clip != runningSound)
                {
                    AudioSource.clip = runningSound;
                    AudioSource.volume = 1.5f;
                    AudioSource.loop = true;
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

    void BasicAttacks()
    {
        int attackType = Random.Range(0, 3); // Randomly choose between two attack types (0 or 1 or 2)
        switch (attackType)
        {
            case 0:
                Animator.SetTrigger("attack_1");
                PlayAttackSound(attackSound1);
                Invoke(nameof(EndAttack), 2.56f);
                break;
            case 1:
                Animator.SetTrigger("attack_2");
                PlayAttackSound(attackSound2);
                Invoke(nameof(EndAttack), 2f);
                break;
            case 2:
                Animator.SetTrigger("attack_3");
                PlayAttackSound(attackSound3);
                Invoke(nameof(EndAttack), 3.8f);
                break;
        }
    }

    void PlayAttackSound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.Stop(); // Stop any current sound
            AudioSource.clip = clip;
            AudioSource.loop = false;
            AudioSource.volume = 1f;
            AudioSource.Play();
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
