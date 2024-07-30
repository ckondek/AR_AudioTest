using UnityEngine;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

public class MQTTSender : MonoBehaviour
{
    private MqttClient client;
    public string brokerAddress = "test.mosquitto.org"; // IP-Adresse deines Raspberry Pi MQTT-Brokers
    public string topic = "derfabs/unity/scene/load"; // Das Topic, auf dem die Nachricht gesendet wird
    public string[] scenesToLoad; // Liste der zu ladenden Szenen
    private int currentSceneIndex = 0;

    void Start()
    {
        client = new MqttClient(brokerAddress);
        string clientId = System.Guid.NewGuid().ToString();
        client.Connect(clientId);
        Debug.Log("Connected to MQTT Broker.");
    }

    void Update()
    {
        // Prüft, ob die Taste 'P' gedrückt wird
        if (Input.GetKeyDown(KeyCode.P))
        {
            SendLoadSceneCommand();
        }
    }

    private void SendLoadSceneCommand()
    {
        if (currentSceneIndex < scenesToLoad.Length)
        {
            string sceneName = scenesToLoad[currentSceneIndex];
            client.Publish(topic, Encoding.UTF8.GetBytes(sceneName));
            Debug.Log("Sent MQTT message to load scene: " + sceneName);
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("All scenes have been queued for loading.");
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
        }
    }
}
