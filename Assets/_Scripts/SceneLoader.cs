using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour
{
    public MQTTManager mqttManager;
   
    public string loadSceneTopic = "unity/scene/load";
    public string unloadSceneTopic = "unity/scene/unload";
    public List<string> scenesToLoad = new List<string>(); // Liste der verfügbaren Szenen zum Laden
    private List<string> loadedScenes = new List<string>(); // Liste der geladenen Szenen
    private int currentSceneIndex = 0; // Index der aktuellen Szene

    private string sceneToLoad; // Name der Szene, die geladen werden soll
    private string sceneToUnload; // Name der Szene, die entladen werden soll
    private bool requestLoad = false;
    private bool requestUnload = false;
    private bool requestLoadNumber = false;
    public static event Action<Scene> OnSceneLoadedEvent; // Neues Event für geladene Szenen


    void Start()
    {
        if (mqttManager == null)
        {
            Debug.LogError("MQTTManager not assigned.");
            return;
        }

        mqttManager.OnMqttMessageReceived += OnMqttMessageReceived;

        // Die aktuelle Szene zur Liste der geladenen Szenen hinzufügen
        loadedScenes.Add(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        // Überprüfe, ob eine Szene geladen werden soll
        if (requestLoad && !string.IsNullOrEmpty(sceneToLoad))
        {
            LoadScene(sceneToLoad);
            requestLoad = false;
        }

        // Überprüfe, ob eine Szene entladen werden soll
        if (requestUnload && !string.IsNullOrEmpty(sceneToUnload))
        {
            UnloadScene(sceneToUnload);
            requestUnload = false;
        }

        // Tasteneingaben für manuelle Steuerung
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadLastScene();
        }

        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                LoadSceneByNumber(i);
                requestLoadNumber = false;
            }
        }

        
    }


    private void OnMqttMessageReceived(string topic, string message)
    {
        if (topic == loadSceneTopic)
        {
            sceneToLoad = message;
            requestLoad = true;
        }
        else if (topic == unloadSceneTopic)
        {
            sceneToUnload = message;
            requestUnload = true;
        }
    }

private void LoadNextScene()
{
    if (currentSceneIndex < scenesToLoad.Count)
    {
        string sceneName = scenesToLoad[currentSceneIndex];
        if (!loadedScenes.Contains(sceneName))
        {
            // MQTT-Nachricht senden
            mqttManager.PublishMessage(loadSceneTopic, sceneName);

            // Vorherige Szene entladen, außer wenn die nächste Szene die gleiche wie die aktuelle ist
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                UnloadLastScene();
            }

            // Neue Szene laden
            sceneToLoad = sceneName;
            requestLoad = true;
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("Scene " + sceneName + " is already loaded.");
        }
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
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedScenes.Add(sceneName);
            Debug.Log("Loaded scene: " + sceneName);
            
        }   
          
    }

    private void UnloadLastScene()
    {
        if (loadedScenes.Count > 1) // Sicherstellen, dass wir nie die erste Szene entladen
        {
            string sceneName = loadedScenes[loadedScenes.Count - 1];
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                mqttManager.PublishMessage(unloadSceneTopic, sceneName);
                sceneToUnload = sceneName;
                requestUnload = true;
                Debug.Log("Last Scene Unloaded");
            }
         
        }
        else
        {
            Debug.Log("Cannot unload the initial scene.");
        }
    }

    private void UnloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            loadedScenes.Remove(sceneName);
            Debug.Log("Unloaded scene: " + sceneName);
        }
       
    }
 private void LoadSceneByNumber(int sceneIndex)
{
    if (sceneIndex < SceneManager.sceneCountInBuildSettings)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        // Debugging: Ausgabe des vollständigen Szenenpfads
        Debug.Log("Szenenpfad für Index " + sceneIndex + ": " + scenePath);
        Debug.Log("Extrahierter Szenenname: " + sceneName);

        // Überprüfen, ob die Szene bereits geladen ist
        if (loadedScenes.Contains(sceneName))
        {
            Debug.Log("Die Szene " + sceneName + " ist bereits geladen.");
            return; // Die Szene ist bereits geladen, nichts tun
        }

        // MQTT-Nachricht senden, um die Szene zu laden
        mqttManager.PublishMessage(loadSceneTopic, sceneName);

        // Vorherige Szene entladen, außer wenn die neue Szene die gleiche wie die aktuelle ist
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            UnloadLastScene();
        }

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


    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        SceneManager.SetActiveScene(scene);
       // Diese Methode kann verwendet werden, um die nächste Szene zu positionieren
        ScenePositioner scenePositioner = FindObjectOfType<ScenePositioner>();
        if (scenePositioner != null)
        {
            scenePositioner.PositionSceneToImage();
        }
    }
    
    
    }


