using UnityEngine;

public class PlaySound : StateMachineBehaviour
{
    public AudioClip sound;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;
    public bool loop = false;
    public bool stopLoopOnExit = true;

    public float playDelay = 0.25f;

    private float timeSinceEnter = 0f;
    private bool hasDelayedSoundPlayed = false;
    private AudioSource loopAudioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (loop)
        {
            loopAudioSource = animator.gameObject.AddComponent<AudioSource>();
            loopAudioSource.clip = sound;
            loopAudioSource.volume = volume;
            loopAudioSource.loop = true;
            loopAudioSource.Play();
        }
        else if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(sound, animator.transform.position, volume);
        }

        timeSinceEnter = 0f;
        hasDelayedSoundPlayed = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasDelayedSoundPlayed && playAfterDelay && !loop)
        {
            timeSinceEnter += Time.deltaTime;
            if (timeSinceEnter >= playDelay)
            {
                AudioSource.PlayClipAtPoint(sound, animator.transform.position, volume);
                hasDelayedSoundPlayed = true;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (loop && stopLoopOnExit && loopAudioSource != null)
        {
            loopAudioSource.Stop();
            GameObject.Destroy(loopAudioSource); // Clean up the component
        }

        if (playOnExit && !loop)
        {
            AudioSource.PlayClipAtPoint(sound, animator.transform.position, volume);
        }
    }
}

