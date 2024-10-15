using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationSoundHandler : MonoBehaviour
{
    public Animator cubeAAnimator;
    public string animationTriggerEnter;
    public string animationTriggerExit;

    public AudioClip enterSoundClip; // Sound to play on trigger enter
    public AudioClip exitSoundClip; // Sound to play on trigger exit

    public int framesToWait;

    private bool inCollisionSphere = false;

    AudioSource _AudioSource;

    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Trigger animation 1 on Cube A
            cubeAAnimator.SetTrigger(animationTriggerEnter);
            inCollisionSphere = true;
        }
        StartCoroutine(DelaySoundTriggerEnter());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Trigger animation 2 on Cube A
            cubeAAnimator.SetTrigger(animationTriggerExit);
            inCollisionSphere = false;
        }
        StartCoroutine(DelaySoundTriggerExit());
    }

    IEnumerator DelaySoundTriggerEnter()
    {
        int framesToWaitLocal1 = framesToWait;
        while (framesToWaitLocal1 > 0)
        {
            yield return null; // Wait for one frame
            framesToWaitLocal1--;
        }

        // Play the sound after the delay
        _AudioSource.Stop();
        _AudioSource.clip = enterSoundClip;
        _AudioSource.Play();
    }

    IEnumerator DelaySoundTriggerExit()
    {
        int framesToWaitLocal2 = framesToWait;
        while (framesToWaitLocal2 > 0)
        {
            yield return null; // Wait for one frame
            framesToWaitLocal2--;
        }

        // Play the sound after the delay
        _AudioSource.Stop();
        _AudioSource.clip = exitSoundClip;
        _AudioSource.Play();
    }

    void Update()
    {
        // Optionally, you can perform continuous actions while Cube A is within the sphere
        if (inCollisionSphere)
        {
            // Perform actions...
        }
    }
}
