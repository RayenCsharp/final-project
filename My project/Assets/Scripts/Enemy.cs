using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Roaming, // Enemy is roaming
        Chasing, // Enemy is chasing the player
    }
    private State currentState; // Current state of the enemy


    [SerializeField] private float speed = 5f; // Speed of the enemy
    private Rigidbody rb; // Rigidbody component of the enemy

    private Vector3 startingPosition; // Starting position of the enemy
    private Vector3 roamingPostion; // Roaming position of the enemy

    private float roamingRange = 10f; // Range within which the enemy can roam
    private float waitTime;
    private float waitTimer; // Timer to track roaming time
    private bool enemystoped; // Flag to check if the enemy is roaming

    public DetectionZone facingobstacle; // Reference to the detection zone for obstacles


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
        switch (currentState)
        {
            case State.Roaming:
                if (Vector3.Distance(transform.position, roamingPostion) < 0.1f)
                {
                    float newWaitTime = 1f; // Generate a random wait time between 1 and 3 seconds
                    ManegingRoamingState(newWaitTime); // Manage the roaming state when the enemy reaches the roaming position
                }
                else if (facingobstacle.detectedColliders.Count > 0) // Check if there are any detected colliders in the detection zone
                {
                    float newWaitTime = 0.1f; // Generate a random wait time between 1 and 3 seconds
                    ManegingRoamingState(newWaitTime); // Manage the roaming state when an obstacle is detected
                }
                break;
            case State.Chasing:
                // Logic for chasing the player can be added here
                break;
        }

    }

    void FixedUpdate()
    {
       if (!enemystoped) // Check if the enemy is not stopped
       {
            Vector3 direction = (roamingPostion - transform.position).normalized; // Calculate the direction towards the roaming position
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime); // Move the enemy towards the roaming position
            if (direction != Vector3.zero) // Check if the direction is not zero to avoid unnecessary rotation
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate the rotation towards the direction
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * speed)); // Smoothly rotate the enemy towards the direction
            }
        }
    }

    void SetNewRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized; // Generate a random direction
        roamingPostion = startingPosition + randomDirection * Random.Range(0f, roamingRange); // Set the new roaming position based on the random direction and range
    }

    void ManegingRoamingState(float newWaitTime)
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
