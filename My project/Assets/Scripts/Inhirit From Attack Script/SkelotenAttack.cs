using UnityEngine;

public class SkelotenAttack : Attack
{
    [SerializeField] private int damage = 30;
    [SerializeField] private LayerMask targetLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            Debug.Log("Skeloten Attack Hit: " + other.gameObject.name);
            ApplyDamage(other, damage);
        }
    }
}

