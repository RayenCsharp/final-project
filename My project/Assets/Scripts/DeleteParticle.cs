using UnityEngine;

public class DeleteParticle : MonoBehaviour
{
    [SerializeField] private float lifetime; // Time in seconds before the particle is destroyed
    private float timer = 0f; // Timer to track the lifetime of the particle


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Increment the timer by the time since the last frame
        if (timer >= lifetime) // Check if the timer has reached the lifetime limit
        {
            Destroy(gameObject); // Destroy the particle game object
        }
    }
}
