using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Roaming, // Enemy is roaming
        Chasing, // Enemy is chasing the player
        attacking // Enemy is attacking the player
    }
    [SerializeField]private State currentState; // Current state of the enemy


    [SerializeField] private float speed = 5f; // Speed of the enemy
    private Rigidbody rb; // Rigidbody component of the enemy

    private Vector3 startingPosition; // Starting position of the enemy
    private Vector3 roamingPostion; // Roaming position of the enemy
    private Vector3 chasingPostion; // Chasing position of the enemy

    private float roamingRange = 10f; // Range within which the enemy can roam
    private float waitTime;
    private float waitTimer; // Timer to track roaming time
    private bool enemystoped; // Flag to check if the enemy is roaming

    public DetectionZone obstacleDetectionZone; // Reference to the detection zone for obstacles
    public DetectionZone targetingZone; // Reference to the detection zone for targeting
    public DetectionZone attackDetectionZone; // Reference to the detection zone for attack detection


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Roaming; // Set the initial state to Roaming
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        startingPosition = transform.position; // Set the starting position to the enemy's current position
        SetNewRoamingPosition(); // Set the initial roaming position
    }

    // Update is called once per frame
    void Update()
    {
        if (attackDetectionZone.detectedColliders.Count > 0)
        {
            currentState = State.attacking;
        }
        else if (targetingZone.detectedColliders.Count > 0)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Roaming;
        }

        switch (currentState)
        {
            case State.Roaming:
                if (Vector3.Distance(transform.position, roamingPostion) < 0.1f)
                {
                    float newWaitTime = 1f; // Generate a random wait time between 1 and 3 seconds
                    ManageRoamingState(newWaitTime); // Manage the roaming state when the enemy reaches the roaming position
                }
                else if (obstacleDetectionZone.detectedColliders.Count > 0) // Check if there are any detected colliders in the detection zone
                {
                    float newWaitTime = 0.1f; // Generate a random wait time between 1 and 3 seconds
                    ManageRoamingState(newWaitTime); // Manage the roaming state when an obstacle is detected
                }
                break;
            case State.Chasing:
                Vector3 target = targetingZone.detectedColliders[0].transform.position; // Get the position of the first detected collider in the targeting zone
                chasingPostion = (target - transform.position).normalized; // Calculate the direction towards the target
                break;
            case State.attacking:
                Debug.Log("Enemy is attacking!"); // Log a message when the enemy is in the attacking state
                break;
        }

    }

    void FixedUpdate()
    {
        if (!enemystoped && currentState == State.Roaming) // Check if the enemy is not stopped
        {
            Vector3 direction = (roamingPostion - transform.position).normalized; // Calculate the direction towards the roaming position
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime); // Move the enemy towards the roaming position
            if (direction != Vector3.zero) // Check if the direction is not zero to avoid unnecessary rotation
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate the rotation towards the direction
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * speed)); // Smoothly rotate the enemy towards the direction
            }
        }
        else if (currentState == State.Chasing) // Check if the enemy is in the Chasing state and there are detected colliders in the targeting zone
        {
            rb.MovePosition(transform.position + chasingPostion * speed * Time.fixedDeltaTime); // Move the enemy towards the target
            Quaternion lookRotation = Quaternion.LookRotation(chasingPostion); // Calculate the rotation towards the direction
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * speed)); // Smoothly rotate the enemy towards the direction

        }
    }

    void SetNewRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized; // Generate a random direction
        roamingPostion = startingPosition + randomDirection * Random.Range(0f, roamingRange); // Set the new roaming position based on the random direction and range
    }

    void ManageRoamingState(float newWaitTime)
    {
        enemystoped = true; // Set the enemy to stopped state if an obstacle is detected
        waitTimer += Time.deltaTime; // Increment the wait timer
        waitTime = newWaitTime; // Set a short wait time when an obstacle is detected
        if (waitTimer >= waitTime)
        {
            enemystoped = false; // Reset the stopped state after waiting
            waitTimer = 0f;
            SetNewRoamingPosition(); // Set a new roaming position when an obstacle is detected
        }
    }
}
