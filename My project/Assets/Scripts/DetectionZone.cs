using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private Collider col;

    public List<Collider> detectedColliders = new List<Collider>();

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider collision)
    {
        detectedColliders.Add(collision);
    }

    void OnTriggerExit(Collider collision)
    {

        detectedColliders.Remove(collision);
    }
}
