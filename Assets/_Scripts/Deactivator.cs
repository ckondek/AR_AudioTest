using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    public AudioSource[] audioSources;

    void Start()
    {
        // Holen alle AudioSource-Komponenten auf dem GameObject und seinen Kindern
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    void Update()
    {
        // Überprüfen, ob eine der AudioSources spielt
        bool isPlaying = false;
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                isPlaying = true;
                break;
            }
        }

        // Wenn keine AudioSource spielt, deaktiviere das GameObject
        if (!isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
