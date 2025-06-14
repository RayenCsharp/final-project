using UnityEngine;

public class PlayerSwordAttack : Attack
{
    [SerializeField] private int damage;
    [SerializeField] private LayerMask targetLayers;


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            Debug.Log("Player Sword Attack Hit: " + other.gameObject.name);
            ApplyDamage(other, damage);
        }
    }
}
