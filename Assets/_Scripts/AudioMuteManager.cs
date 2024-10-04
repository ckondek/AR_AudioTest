using UnityEngine;

public class AudioMuteManager : MonoBehaviour
{
    public AudioSource currentAudioSource;
    public AudioSource[] allAudioSources;

    void Start()
    {
        // Die AudioSource-Komponente des aktuellen GameObjects holen
        currentAudioSource = GetComponent<AudioSource>();

        // Alle AudioSource-Komponenten im Spiel finden
        allAudioSources = FindObjectsOfType<AudioSource>();
    }

    void Update()
    { // Alle AudioSource-Komponenten im Spiel finden
        allAudioSources = FindObjectsOfType<AudioSource>();
        if (currentAudioSource.isPlaying)
        {
            // Alle anderen AudioSources stummschalten
            foreach (AudioSource audioSource in allAudioSources)
            {
                if (audioSource != currentAudioSource)
                {
                    audioSource.mute = true;
                }
            }
        }
        else
        {
            // Alle anderen AudioSources wieder aktivieren, wenn die aktuelle AudioSource nicht mehr spielt
            foreach (AudioSource audioSource in allAudioSources)
            {
                audioSource.mute = false;
            }
        }
    }
}