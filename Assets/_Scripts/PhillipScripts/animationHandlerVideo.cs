using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class animationHandlerVideo : MonoBehaviour
{
    public Animator cubeAAnimator;
    public string animationTriggerEnter;
    public string animationTriggerExit;
    public bool isEnteringSound = false;
    public int framesToWait = 70;
    public VideoPlayer videoPlayer;  // Add a reference to the VideoPlayer component

    private bool inCollisionSphere = false;
    private AudioSource _AudioSource;

    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        if (videoPlayer != null)
        {
            videoPlayer.Stop();  // Ensure the video is stopped initially
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Trigger animation 1 on Cube A
            cubeAAnimator.SetTrigger(animationTriggerEnter);
            inCollisionSphere = true;

            if (videoPlayer != null)
            {
                videoPlayer.Play();  // Activate the VideoPlayer
            }
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

            if (videoPlayer != null)
            {
                videoPlayer.Stop();  // Deactivate the VideoPlayer
            }
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
