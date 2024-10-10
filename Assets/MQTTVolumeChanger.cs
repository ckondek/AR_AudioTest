using UnityEngine;

public class MQTTVolumeChanger : MonoBehaviour
{
    public MQTTManager mqttManager; // Verweis auf den MQTTManager
    public string volumeUpTopic = "unity/volume/lauter";  // MQTT Topic zum Erhöhen der Lautstärke
    public string volumeDownTopic = "unity/volume/leiser"; // MQTT Topic zum Verringern der Lautstärke

    public float volume = 1.0f; // Aktuelle Lautstärke (von 0.0f bis 1.0f)
    private const float volumeChangeAmount = 0.05f; // Menge, um die die Lautstärke geändert wird
public AudioSource[] allAudioSources;
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
    { allAudioSources = FindObjectsOfType<AudioSource>();
        // Lautstärkeänderung mit den Pfeiltasten
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeVolume(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeVolume(-1);
        }
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = volume;
        }
    }

    private void ChangeVolume(int direction)
    {
        // Berechnung der neuen Lautstärke
        volume += direction * volumeChangeAmount;
        volume = Mathf.Clamp(volume, 0.0f, 1.0f); // Lautstärke auf den Bereich von 0.0 bis 1.0 begrenzen

        // Sende die aktualisierte Lautstärke über MQTT
        if (direction > 0)
        {
            mqttManager.PublishMessage(volumeUpTopic, volume.ToString());
        }
        else
        {
            mqttManager.PublishMessage(volumeDownTopic, volume.ToString());
        }

        Debug.Log("Aktuelle Lautstärke: " + volume);
    }

    private void OnMessageReceived(string topic, string message)
    {
        // Hier kann der Code zum Verarbeiten empfangener Nachrichten implementiert werden, falls nötig.
        Debug.Log($"Nachricht empfangen auf Thema: {topic}, Nachricht: {message}");
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
