using UnityEngine;

public class ParticleEffectPlayer : MonoBehaviour
{

    [SerializeField]private PlayerSpecialAttack playerSpecialAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSpecialAttack = GetComponentInChildren<PlayerSpecialAttack>();
    }

    public void SpawnEffect()
    {
        if (playerSpecialAttack != null)
        {
            playerSpecialAttack.SpawnHitEffect();
        }
        else
        {
            Debug.LogWarning("PlayerSpecialAttack component not found on " + gameObject.name);
        }
    }

}
