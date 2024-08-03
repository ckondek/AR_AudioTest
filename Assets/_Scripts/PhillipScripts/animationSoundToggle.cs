using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationSoundToggle : MonoBehaviour
{
    public Animator cubeAAnimator;
    public string animation1Trigger;
    public string animation2Trigger;

    public AudioClip sound1;
    public AudioClip sound2;

    public float animation1Cooldown = 2f;
    public float animation2Cooldown = 3f;

    private bool canTriggerAnimation1 = true;
    private bool canTriggerAnimation2 = true;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (canTriggerAnimation1)
            {
                PlayAnimationWithCooldown(animation1Trigger, sound1, animation1Cooldown);
            }
            else if (canTriggerAnimation2)
            {
                PlayAnimationWithCooldown(animation2Trigger, sound2, animation2Cooldown);
            }
        }
    }

    void PlayAnimationWithCooldown(string animationTrigger, AudioClip soundClip, float cooldown)
    {
        cubeAAnimator.SetTrigger(animationTrigger);
        audioSource.clip = soundClip;
        audioSource.Play();

        if (animationTrigger == animation1Trigger)
        {
            canTriggerAnimation1 = false;
            StartCoroutine(EnableTriggerAfterCooldown(animationTrigger, animation1Cooldown));
        }
        else if (animationTrigger == animation2Trigger)
        {
            canTriggerAnimation2 = false;
            StartCoroutine(EnableTriggerAfterCooldown(animationTrigger, animation2Cooldown));
        }
    }

    IEnumerator EnableTriggerAfterCooldown(string animationTrigger, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        if (animationTrigger == animation1Trigger)
        {
            canTriggerAnimation1 = true;
        }
        else if (animationTrigger == animation2Trigger)
        {
            canTriggerAnimation2 = true;
        }
    }
}