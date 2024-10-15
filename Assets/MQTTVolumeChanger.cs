using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTVolumeChanger : MonoBehaviour
{
    public MQTTManager mqttManager; // Verweis auf den MQTTManager
    public string volumeTopic = "unity/volume";  // MQTT Topic zum Verändern der Lautstärke
    public AudioManager audioManager; // Verweis auf den AudioManager
    
    public float volume = 0.5f; // Aktuelle Lautstärke (von 0.0f bis 1.0f)
    private const float volumeChangeAmount = 0.05f; // Menge, um die die Lautstärke geändert wird
    private Queue<Action> actionQueue = new Queue<Action>(); // Queue für Hauptthread-Aktionen

    void Start()
    {
        // MQTTManager holen
        if (mqttManager == null)
        {
            mqttManager = FindObjectOfType<MQTTManager>();
        }

    

        if (mqttManager == null)
        {
            Debug.LogError("MQTT Manager nicht gefunden!");
        }
        else
        {
            mqttManager.OnMqttMessageReceived += OnMessageReceived; // Abonniere Topic
        }
    }

    void Update()
    {
         if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        // Verarbeite die Aktionen im Hauptthread
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }

        // Lautstärkeänderung über Tastatur
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeVolume(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeVolume(-1);
        }
    }

    public void ChangeVolume(int direction)
    {
        // Lautstärke berechnen
        volume += direction * volumeChangeAmount;
        volume = Mathf.Clamp(volume, 0.0f, 1.0f); // Begrenzen zwischen 0 und 1

        // Benachrichtige den AudioManager über die neue Lautstärke
        if (audioManager != null)
        {
            audioManager.UpdateAudioSourcesVolume(volume);
        }

        // MQTT-Nachricht senden
        mqttManager.PublishMessage(volumeTopic, volume.ToString("F2"));
        Debug.Log("Lautstärke geändert: " + volume);
    }

    private void OnMessageReceived(string topic, string message)
    {
        if (topic == volumeTopic)
        {
            lock (actionQueue)
            {
                actionQueue.Enqueue(() => StartCoroutine(HandleVolumeChange(message)));
            }
        }
    }

    private IEnumerator HandleVolumeChange(string message)
    {
        if (float.TryParse(message, out float newVolume))
        {
            newVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);
            volume = newVolume; // Aktualisiere die Lautstärke

            // Benachrichtige den AudioManager über die neue Lautstärke
            if (audioManager != null)
            {
                audioManager.UpdateAudioSourcesVolume(volume);
            }

            Debug.Log("MQTT Lautstärke empfangen: " + volume);
        }
        else
        {
            Debug.LogError("Fehlerhafte Lautstärkenachricht: " + message);
        }

        yield return null;
    }

    void OnDestroy()
    {
        // Entferne Event-Handler bei Zerstörung
        if (mqttManager != null)
        {
            mqttManager.OnMqttMessageReceived -= OnMessageReceived;
        }
    }
}
