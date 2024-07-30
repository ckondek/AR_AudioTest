using UnityEngine;
using System;
using System.Text;
using System.Linq; // Füge diesen Namespace hinzu
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTManager : MonoBehaviour
{
    public MqttClient client;
    public string brokerAddress = "test.mosquitto.org"; // IP-Adresse deines Raspberry Pi MQTT-Brokers
    private string clientId = Guid.NewGuid().ToString(); // Eindeutige Client-ID
    public string[] topics; // Liste der Topics, die abonniert werden sollen
    public bool IsConnected => client != null && client.IsConnected;
    public UnityMainThreadDispatcher UnityMainThreadDispatcher;

    public delegate void MqttMessageReceivedHandler(string topic, string message);
    public event MqttMessageReceivedHandler OnMqttMessageReceived;
    

    void Start()
    {
        client = new MqttClient(brokerAddress);
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        client.Connect(clientId);

        if (topics != null && topics.Length > 0)
        {
            // Füge den using System.Linq Namespace hinzu, um die Select-Methode zu verwenden
            byte[] qosLevels = new byte[topics.Length];
            for (int i = 0; i < qosLevels.Length; i++)
            {
                qosLevels[i] = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
            }

            client.Subscribe(topics, qosLevels);
            Debug.Log("Subscribed to MQTT topics: " + string.Join(", ", topics));
        }
        else
        {
            Debug.LogWarning("No MQTT topics to subscribe to.");
        }
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string topic = e.Topic;
        string message = Encoding.UTF8.GetString(e.Message);
        // Benachrichtige den Dispatcher, um die Nachricht im Hauptthread zu verarbeiten
    UnityMainThreadDispatcher.Enqueue(() => 
    {
        OnMqttMessageReceived?.Invoke(topic, message);
    });
        Debug.Log("Received message: " + message + " on topic: " + topic);

        OnMqttMessageReceived?.Invoke(topic, message);
    }

    public void PublishMessage(string topic, string message)
    {
        client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
        }
    }
}
