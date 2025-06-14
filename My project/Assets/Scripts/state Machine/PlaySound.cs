using UnityEngine;

public class PlaySound : StateMachineBehaviour
{
    public AudioClip sound;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;

    //Delayer sound timer
    public float playDelay = 0.25f;
    private float timeSinceEnter = 0f;
    private bool hasDelayedSoundPlayed = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(sound, animator.transform.position, volume);
        }
        timeSinceEnter = 0f;
        hasDelayedSoundPlayed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasDelayedSoundPlayed && playAfterDelay)
        {
            timeSinceEnter += Time.deltaTime;
            if (timeSinceEnter >= playDelay)
            {
                AudioSource.PlayClipAtPoint(sound, animator.transform.position, volume);
                hasDelayedSoundPlayed = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(sound, animator.transform.position, volume);
        }
    }


}
