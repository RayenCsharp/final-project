using UnityEngine;

public class ChargeAttack : Attack
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int damage = 20;
    [SerializeField] private float KnockbackForce = 15f;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            ApplyDamage(other, damage);
            ApplyKnockback(other, CalculateKnockbackDirection(other));
        }
    }

    private Vector3 CalculateKnockbackDirection(Collider target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0; // Keep the knockback horizontal
        Vector3 finalKnockback = new Vector3(direction.x, 2f, direction.z).normalized * KnockbackForce;
        return finalKnockback;
    }
}
