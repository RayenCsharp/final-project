using UnityEngine;

public class ControlMusic : MonoBehaviour
{
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip bossMusic;

    private AudioSource audioSource;
    private bool bossMusicPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainMusic;

        if (DataHolder.Instance != null)
        {
            audioSource.volume = DataHolder.Instance.Volume;
        }

        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null && !bossMusicPlaying)
        {
            audioSource.clip = bossMusic;
            audioSource.Play();
            bossMusicPlaying = true;
        }
    }
}

