using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip soundClip; // The sound clip to play
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component if it doesn't already exist
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
        audioSource.loop = true; // Set to true if you want the sound to loop
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the main camera
        if (other.CompareTag("MainCamera"))
        {
            audioSource.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is the main camera
        if (other.CompareTag("MainCamera"))
        {
            audioSource.Stop();
        }
    }
}
