using System.Collections;
using UnityEngine;

public class animationHandler : MonoBehaviour
{
    public Animator cubeAAnimator;
    public string animationTriggerEnter;
    public string animationTriggerExit;
    public bool isEnteringSound = false;
    public int framesToWait = 70;

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
        if (isEnteringSound)
        {
            // Delay sound trigger by 30 frames per second
            StartCoroutine(DelaySoundTrigger());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Trigger animation 2 on Cube A
            cubeAAnimator.SetTrigger(animationTriggerExit);
            inCollisionSphere = false;
        }
        if (!isEnteringSound)
        {
            // Delay sound trigger by 30 frames per second
            StartCoroutine(DelaySoundTrigger());
        }
    }

    IEnumerator DelaySoundTrigger()
    {
        int framesToWaitLocal = framesToWait;
        while (framesToWaitLocal > 0)
        {
            yield return null; // Wait for one frame
            framesToWaitLocal--;
        }

        // Play the sound after the delay
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
