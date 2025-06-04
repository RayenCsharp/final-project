using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [Tooltip("Only colliders on these layers will be detected.")]
    [SerializeField] private LayerMask detectionLayer; // Allows configuration in the Inspector

    public List<Collider> detectedColliders = new List<Collider>();

    private Collider col; // Reference to the Collider component

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only detect colliders on the specified layer(s)
        if (((1 << other.gameObject.layer) & detectionLayer) != 0)
        {
            if (!detectedColliders.Contains(other))
            {
                detectedColliders.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (detectedColliders.Contains(other))
        {
            detectedColliders.Remove(other);
        }
    }
}