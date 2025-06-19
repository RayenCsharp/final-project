using UnityEngine;

public class GaintAttack_3 : Attack
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int damage = 90;
    [SerializeField] private float KnockbackForce = 10f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Damageble Damageble;

    void Start()
    {
        Damageble = GetComponentInParent<Damageble>();
    }

    void Update()
    {
        if (Damageble.CurrentHealth == (Damageble.MaxHealth / 2))
        {
            Debug.Log("Giant is at half health, increasing damage of attack 3");
            damage = 150;
        }
    }



    public void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, spawnPos);
        }
    }

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