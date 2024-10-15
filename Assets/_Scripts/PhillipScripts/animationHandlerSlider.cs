using System.Collections;
using UnityEngine;

public class animationHandlerSlider : MonoBehaviour
{
    public Animator animator;
    public AudioClip[] audioClips; // Array to hold the audio clips for each animation
    private AudioSource audioSource;
    private int currentAnimationIndex = 0;
    private bool isAnimationPlaying = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !isAnimationPlaying)
        {
            StartCoroutine(PlayAnimationSequence());
        }
    }

    private IEnumerator PlayAnimationSequence()
    {
        isAnimationPlaying = true;

        // Play the current animation
        string animationName = "Animation" + (currentAnimationIndex + 1);
        animator.Play(animationName);

        // Play the corresponding audio clip
        if (audioClips.Length > currentAnimationIndex)
        {
            audioSource.clip = audioClips[currentAnimationIndex];
            audioSource.Play();
        }

        // Wait for the animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Move to the next animation in the sequence
        currentAnimationIndex = (currentAnimationIndex + 1) % 3;

        isAnimationPlaying = false;
    }
}
