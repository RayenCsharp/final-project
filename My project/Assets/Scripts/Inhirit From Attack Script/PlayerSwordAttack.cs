using UnityEngine;

public class PlayerSwordAttack : Attack
{
    [SerializeField] private int damage = 35;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        if (playerController.currentPowerUp == PowerUpType.DoubleDamage)
        {
            damage = 70; // Double the damage if the player has the double damage power-up
        }
        else
        {
            damage = 35; // Reset to normal damage
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            Debug.Log("Player Sword Attack Hit: " + other.gameObject.name);
            ApplyDamage(other, damage);
        }
    }
}
