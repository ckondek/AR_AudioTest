using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public MQTTManager mqttManager;
    public string loadSceneTopic = "unity/scene/load";
    public string unloadSceneTopic = "unity/scene/unload";
    public List<string> scenesToLoad = new List<string>(); // Liste der verfügbaren Szenen zum Laden
    private List<string> loadedScenes = new List<string>(); // Liste der geladenen Szenen
    private int currentSceneIndex = 0; // Index der aktuellen Szene

    void Start()
    {
        if (mqttManager == null)
        {
            Debug.LogError("MQTTManager not assigned.");
            return;
        }

        mqttManager.OnMqttMessageReceived += OnMqttMessageReceived;

        // Die Basic-Szene zur Liste der geladenen Szenen hinzufügen
        string initialScene = SceneManager.GetActiveScene().name;
        loadedScenes.Add(initialScene);  // Hier wird die Basic-Szene nicht entladen
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        // Taste "L" für das Laden der nächsten Szene
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }

        // Taste "U" für das Entladen der letzten Szene
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadLastScene();
        }

        // Nummerntasten für das Laden einer bestimmten Szene
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                LoadSceneByNumber(i);
            }
        }
    }

    private void OnMqttMessageReceived(string topic, string message)
    {
        if (topic == loadSceneTopic)
        {
            LoadScene(message);
        }
        else if (topic == unloadSceneTopic)
        {
            UnloadScene(message);
        }
    }

    private void LoadNextScene()
    {
        if (currentSceneIndex < scenesToLoad.Count)
        {
            string nextScene = scenesToLoad[currentSceneIndex];
            currentSceneIndex++;

            LoadScene(nextScene);
        }
        else
        {
            Debug.Log("No more scenes to load.");
        }
    }

    private void LoadScene(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            // MQTT-Befehl senden, um die Szene zu laden
            mqttManager.PublishMessage(loadSceneTopic, sceneName);

            // Vorherige Szene entladen (außer 'Basic')
            UnloadLastScene();

            // Neue Szene laden
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedScenes.Add(sceneName);
            Debug.Log("Loaded scene: " + sceneName);
        }
    }

    private void LoadSceneByNumber(int sceneIndex)
    {
        if (sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
             // Abrufen des Szenenpfads
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        
        // Debugging: Ausgabe des vollständigen Szenenpfads
        Debug.Log("Szenenpfad für Index " + sceneIndex + ": " + scenePath);

        // Extrahieren des Szenennamens
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        // Debugging: Ausgabe des extrahierten Szenennamens
        Debug.Log("Extrahierter Szenenname: " + sceneName);
      
            Debug.Log("Lade Szene mit Index: " + sceneIndex + " und Name: " + sceneName);
            Debug.Log("MQTT Topic: " + loadSceneTopic);
            // MQTT-Befehl senden, um die Szene zu laden
            mqttManager.PublishMessage(loadSceneTopic, sceneName);
           
            // Vorherige Szene entladen (außer 'Basic')
            UnloadLastScene();

            // Neue Szene laden
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
            loadedScenes.Add(sceneName);
            Debug.Log("Loaded scene: " + sceneName);
        }
        else
        {
            Debug.LogError("Die Szene mit Index " + sceneIndex + " existiert nicht im Build.");
        }
    }

    private void UnloadLastScene()
    {
        // Die letzte geladene Szene entladen, aber nicht die "Basic"-Szene
        if (loadedScenes.Count > 1 && loadedScenes[loadedScenes.Count - 1] != "Basic")
        {
            string sceneToUnload = loadedScenes[loadedScenes.Count - 1];
            if (SceneManager.GetSceneByName(sceneToUnload).isLoaded)
            {
                // MQTT-Befehl senden, um die Szene zu entladen
                mqttManager.PublishMessage(unloadSceneTopic, sceneToUnload);

                // Entladen der Szene
                SceneManager.UnloadSceneAsync(sceneToUnload);
                loadedScenes.Remove(sceneToUnload);
                Debug.Log("Unloaded scene: " + sceneToUnload);
            }
        }
        else
        {
            Debug.Log("Cannot unload 'Basic' scene.");
        }
    }

    private void UnloadScene(string sceneName)
    {
        // Unload a scene if it's loaded and isn't 'Basic'
        if (SceneManager.GetSceneByName(sceneName).isLoaded && sceneName != "Basic")
        {
            SceneManager.UnloadSceneAsync(sceneName);
            loadedScenes.Remove(sceneName);
            Debug.Log("Unloaded scene: " + sceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene); // Set the newly loaded scene as active
    }
}
