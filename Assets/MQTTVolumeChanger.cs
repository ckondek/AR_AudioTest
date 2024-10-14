using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTVolumeChanger : MonoBehaviour
{
    public MQTTManager mqttManager; // Verweis auf den MQTTManager
    public string volumeUpTopic = "unity/volume/lauter";  // MQTT Topic zum Erhöhen der Lautstärke
    public string volumeDownTopic = "unity/volume/leiser"; // MQTT Topic zum Verringern der Lautstärke

    public float volume = 1.0f; // Aktuelle Lautstärke (von 0.0f bis 1.0f)
    private const float volumeChangeAmount = 0.05f; // Menge, um die die Lautstärke geändert wird
    public GameObject[] allAudioSources;

    // Queue für Nachrichten, die im Hauptthread verarbeitet werden sollen
    private Queue<Action> actionQueue = new Queue<Action>();

    void Start()
    {
        // Überprüfen, ob der MQTTManager zugewiesen wurde, ansonsten suchen
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
            // Abonniere die entsprechenden Topics für Lautstärkeänderungen
            mqttManager.OnMqttMessageReceived += OnMessageReceived;
        }
    }

    void Update()
    {
        allAudioSources = GameObject.FindGameObjectsWithTag("Audio");

        // Lautstärkeänderung mit den Pfeiltasten
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeVolume(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeVolume(-1);
        }

        // Verarbeite alle in die Queue eingereihten Aktionen (nur im Hauptthread)
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }

    private void ChangeVolume(int direction)
    {
        // Berechnung der neuen Lautstärke
        volume += direction * volumeChangeAmount;
        volume = Mathf.Clamp(volume, 0.0f, 1.0f); // Lautstärke auf den Bereich von 0.0 bis 1.0 begrenzen

        // Sende die aktualisierte Lautstärke über MQTT
        //if (direction > 0)
        //{
            mqttManager.PublishMessage(volumeUpTopic, volume.ToString());
        //}
        //else
        //{
        //    mqttManager.PublishMessage(volumeDownTopic, volume.ToString());
        //    Debug.Log("Lautstärke geändert nach unten");
        //}

        Debug.Log("Aktuelle Lautstärke: " + volume);
    }

    private void OnMessageReceived(string topic, string message)
    {
        // Nachricht verarbeiten, falls sie von den relevanten Topics kommt
        Debug.Log($"Nachricht empfangen auf Thema: {topic}, Nachricht: {message}");

        if (topic == volumeUpTopic || topic == volumeDownTopic)
        {
            // Füge die Aktion zur Queue hinzu, damit sie im Hauptthread ausgeführt wird
            lock (actionQueue)
            {
                actionQueue.Enqueue(() => StartCoroutine(HandleVolumeChange(message)));
            }
        }
    }

    private IEnumerator HandleVolumeChange(string message)
    {
        // Konvertiere die Nachricht in einen Float-Wert
        if (float.TryParse(message, out float newVolume))
        {
            newVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f); // Sicherstellen, dass die Lautstärke zwischen 0.0 und 1.0 bleibt
            Debug.Log("Neue Lautstärke aus Nachricht: " + newVolume);

            // Finde alle Audioquellen im Hauptthread und aktualisiere die Lautstärke
            foreach (GameObject audioObject in allAudioSources)
            {
                AudioSource audioSource = audioObject.GetComponent<AudioSource>();

                if (audioSource != null)
                {
                    audioSource.volume = newVolume;
                }
            }
        }
        else
        {
            Debug.LogError("Fehler: Ungültige Lautstärkenachricht erhalten: " + message);
        }

        yield return null;
    }

    void OnDestroy()
    {
        // Entferne den Event-Handler, wenn das Objekt zerstört wird
        if (mqttManager != null)
        {
            mqttManager.OnMqttMessageReceived -= OnMessageReceived;
        }
    }
}
