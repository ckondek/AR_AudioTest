using UnityEngine;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine.SceneManagement;

public class MQTTSceneLoader : MonoBehaviour
{
    private MqttClient client;
    private string brokerAddress = "192.168.1.100"; // IP-Adresse deines Raspberry Pi MQTT-Brokers
    private string[] topics = { "unity/scene/load", "unity/scene/unload", "unity/scene/switch" }; // Beispielthemen

    void Start()
    {
        client = new MqttClient(brokerAddress);
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        string clientId = System.Guid.NewGuid().ToString();
        client.Connect(clientId);

        byte[] qosLevels = new byte[topics.Length];
        for (int i = 0; i < qosLevels.Length; i++)
        {
            qosLevels[i] = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
        }

        client.Subscribe(topics, qosLevels);

        Debug.Log("Connected to MQTT Broker and subscribed to topics.");
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string topic = e.Topic;
        string payload = Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received message: " + payload + " on topic: " + topic);

        switch (topic)
        {
            case "unity/scene/load":
                LoadSceneAdditively(payload);
                break;
            case "unity/scene/unload":
                UnloadScene(payload);
                break;
            case "unity/scene/switch":
                SwitchScene(payload);
                break;
            default:
                Debug.LogWarning("Unhandled topic: " + topic);
                break;
        }
    }

    private void LoadSceneAdditively(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    private void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    private void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
        }
    }
}
