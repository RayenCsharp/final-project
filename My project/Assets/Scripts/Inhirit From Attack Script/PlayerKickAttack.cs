using UnityEngine;

public class PlayerKickAttack : Attack
{

    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int damage = 20;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        if (playerController.currentPowerUp == PowerUpType.DoubleDamage)
        {
            damage = 40; // Double the damage if the player has the double damage power-up
        }
        else
        {
            damage = 20; // Reset to normal damage
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
