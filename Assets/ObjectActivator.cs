using UnityEngine;
using System.Collections;
using System.Text;


public class ObjectActivator : MonoBehaviour
{
    public MQTTManager mqttManager;
    public string listenTopic = "unity/midi/activate";
    public string expectedMessage;
    public GameObject targetObject;

    // Flag, um zu verfolgen, ob das GameObject aktiviert oder deaktiviert werden soll
    private bool shouldActivate;

    void Start()
    {
        if (mqttManager == null)
        {
            mqttManager = FindObjectOfType<MQTTManager>();
        }

        if (mqttManager != null)
        {
            mqttManager.OnMqttMessageReceived += OnMessageReceived; // Event abonnieren
        }
        else
        {
            Debug.LogError("MQTT Manager nicht gefunden!");
        }
    }

    void OnMessageReceived(string topic, string message)
    {
        // Überprüfe, ob das Thema übereinstimmt
        if (topic == listenTopic)
        {
            Debug.Log("Neue Nachricht empfangen: " + message);
            shouldActivate = (message == expectedMessage);
            Debug.Log("should activate" + shouldActivate);
        }
    }

    void Update()
    {
        // Aktivieren oder Deaktivieren des GameObjects im Haupt-Thread
        if (targetObject != null && shouldActivate == true)
        {
            targetObject.SetActive(true);
            Debug.Log(targetObject + "activated");
           
            // Reset the flag to prevent continuous activation/deactivation
            shouldActivate = false;
        }
        else if (targetObject == null)
        {
            Debug.LogError("Kein Ziel-GameObject zugewiesen!");
        }
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
