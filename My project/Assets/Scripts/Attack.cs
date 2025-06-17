using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected void ApplyDamage(Collider target, int damage)
    {
        if (target.CompareTag("Enemy") || target.CompareTag("Boss"))
        {
            Damageble damageble = target.GetComponent<Damageble>();
            if (damageble == null) return;
            damageble.TakeDamage(damage);
        }
        else if (target.CompareTag("Player"))
        {
            Damageble damageble = target.GetComponent<Damageble>();
            if (damageble == null)
            {
                Debug.LogWarning("Player Damageble component not found!"); return;
            }
            damageble.TakeDamage(damage);
        }
    }

    protected void ApplyKnockback(Collider target, Vector3 knockBackForce)
    {
        Rigidbody rb = target.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(knockBackForce, ForceMode.Impulse);
        }

    }
}
