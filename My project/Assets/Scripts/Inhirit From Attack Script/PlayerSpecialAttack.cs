using UnityEngine;

public class PlayerSpecialAttack : Attack
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int damage = 80;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        if (playerController.currentPowerUp == PowerUpType.DoubleDamage)
        {
            damage = 160; // Double the damage if the player has the double damage power-up
        }
        else
        {
            damage = 80; // Reset to normal damage
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
        }
    }
}
