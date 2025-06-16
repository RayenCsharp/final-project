using UnityEngine;

public class ParticleEffectManeger : MonoBehaviour
{
    [SerializeField]private GiantAttack_1 GiantAttack_1;
    [SerializeField]private GaintAttack_2 GiantAttack_2;
    [SerializeField]private GaintAttack_3 GiantAttack_3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GiantAttack_1 = GetComponentInChildren<GiantAttack_1>();
        GiantAttack_2 = GetComponentInChildren<GaintAttack_2>();
        GiantAttack_3 = GetComponentInChildren<GaintAttack_3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiantAttack_1_ParticleEffect()
    {
        GiantAttack_1.SpawnHitEffect();
    }

    public void GiantAttack_2_ParticleEffect()
    {
        GiantAttack_2.SpawnHitEffect();
    }

    public void GiantAttack_3_ParticleEffect()
    {
        GiantAttack_3.SpawnHitEffect();
    }
}
