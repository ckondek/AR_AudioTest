using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Collections.Generic;


public class MQTTActivator : MonoBehaviour
{
    public MQTTManager mqttManager; // Verweis auf den MQTTManager
    public string activationTopic = null; // Topic für Aktivierungsbefehle
    public string deactivationTopic = null; // Topic für Deaktivierungsbefehle
    public GameObject activationObject; 

     private Queue<System.Action> actionsQueue = new Queue<System.Action>();

    void Start()
    {
        if (mqttManager == null)
        {
            Debug.LogError("MQTTManager not assigned.");
            return;
        }
        mqttManager.OnMqttMessageReceived += OnMqttMessageReceived;
    }

    void Update()
    {
           // Verarbeite alle in der Warteschlange befindlichen Aktionen
        while (actionsQueue.Count > 0)
        {
            System.Action action = actionsQueue.Dequeue();
            action.Invoke();
        }

        // Überprüfe, ob die Taste "A" gedrückt wurde
        if (Input.GetKeyDown(KeyCode.A))
        {
            SendMqttMessage(activationTopic, "activate");
        }

        // Überprüfe, ob die Taste "D" gedrückt wurde
        if (Input.GetKeyDown(KeyCode.D))
        {
            SendMqttMessage(deactivationTopic, "deactivate");
        }
    }

    private void SendMqttMessage(string topic, string message)
    {
        if (mqttManager != null && mqttManager.IsConnected)
        {
            mqttManager.PublishMessage(topic, message);
            Debug.Log($"Sent MQTT message to {topic}: {message}");
        }
        else
        {
            Debug.LogWarning("MQTTManager is not connected.");
        }
    }
private void OnMqttMessageReceived(string topic, string message)
    {
        // Füge die Aktion zur Warteschlange hinzu
        if (topic == activationTopic && message == "activate")
        { Debug.Log("Activation Message receiced");
            actionsQueue.Enqueue(() => ActivateObject());
        }
        else if (topic == deactivationTopic && message == "deactivate")
        {
            actionsQueue.Enqueue(() => DeactivateObject());
        }
    }
  
     private void ActivateObject()
    {
        if (!activationObject.activeSelf)
        {
            activationObject.SetActive(true);
            Debug.Log("Object activated.");
        }
    }

    private void DeactivateObject()
    {
        if (activationObject.activeSelf)
        {
            activationObject.SetActive(false);
            Debug.Log("Object deactivated.");
        }
    }

    private void OnDestroy()
    {
        if (mqttManager != null)
        {
            mqttManager.OnMqttMessageReceived -= OnMqttMessageReceived;
        }
    }
}
