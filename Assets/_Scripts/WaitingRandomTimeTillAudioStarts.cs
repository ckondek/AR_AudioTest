using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomAudioPlayer : MonoBehaviour
{
    // Audioquelle, die den Sound abspielt
    public AudioSource audioSource;
    public AudioClip [] clips;
    
    // Zufällig generierte Zeitintervalle (zwischen 1 und 15 Sekunden)
    public float randomTimeInterval;
    
    // Timer zur Verfolgung der verstrichenen Zeit
    private float timeSinceLastPlay= 0f;

    void Start()
    {
         audioSource = this.GetComponent<AudioSource>();
       
      
        // Generiere eine zufällige Zeitspanne zwischen 1 und 15 Sekunden
        randomTimeInterval = Random.Range(1f, 240f);
        
  
    }

    void Update()
    {
        // Zähle die verstrichene Zeit
        timeSinceLastPlay += Time.deltaTime;

        // Wenn die verstrichene Zeit das zufällige Zeitintervall erreicht, spiele das Audio ab
        if (timeSinceLastPlay >= randomTimeInterval)
        {
            // Audio abspielen
            PlayAudio();

        }
    }

    // Funktion zum Abspielen des Audios
    void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = clips[(int)Random.Range(0,clips.Length)];
            audioSource.Play();
            Debug.Log("Audio wird abgespielt!");
        }
    }
}


