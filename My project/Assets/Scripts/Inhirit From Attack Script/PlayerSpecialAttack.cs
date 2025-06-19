using UnityEngine;

public class PlayerSpecialAttack : Attack
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int damage = 100;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        if (playerController.currentPowerUp == PowerUpType.DoubleDamage)
        {
            damage = 200; // Double the damage if the player has the double damage power-up
        }
        else
        {
            damage = 100; // Reset to normal damage
        }
    }


    public void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            Debug.Log("Spawning hit effect at player special attack position.");
            Instantiate(hitEffectPrefab, spawnPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            ApplyDamage(other, damage);
        }
    }
}
