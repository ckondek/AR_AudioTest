using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public MQTTVolumeChanger mqttVolumeChanger; // Referenz zum MQTTVolumeChanger
    public GameObject[] allAudioSources; // Array für alle AudioSources im Spiel

    void Start()
    {
        // Referenz zum MQTTVolumeChanger finden
        if (mqttVolumeChanger == null)
        {
            mqttVolumeChanger = FindObjectOfType<MQTTVolumeChanger>();
        }

        // Initialisierung: Alle Audioquellen im Spiel finden
        allAudioSources = GameObject.FindGameObjectsWithTag("Audio");
    }

    void Update()
    {
        // Sicherstellen, dass der MQTTVolumeChanger existiert
        if (mqttVolumeChanger != null)
        {
            // Wende die Lautstärke des MQTTVolumeChanger auf alle AudioSources an
            UpdateAudioSourcesVolume(mqttVolumeChanger.volume);
        }
    }

    public void UpdateAudioSourcesVolume(float volume)
    {
        foreach (GameObject audioObject in allAudioSources)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = volume; // Setze die Lautstärke
            }
        }

        Debug.Log("AudioSources Lautstärke aktualisiert: " + volume);
    }
}
