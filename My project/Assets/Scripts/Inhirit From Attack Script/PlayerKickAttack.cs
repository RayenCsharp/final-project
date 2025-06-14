using UnityEngine;

public class PlayerKickAttack : Attack
{

    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private int damage = 5; // Force applied to the target upon kick

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            ApplyDamage(other, damage); // Assuming no damage is applied, just knockback
        }
    }
}
