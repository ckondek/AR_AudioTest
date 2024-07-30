using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneDebugLoader : MonoBehaviour
{
    [Header("Scene Management")]
    public List<string> scenesToLoad = new List<string>(); // Liste der zu ladenden Szenen
    public List<string> scenesToUnload = new List<string>(); // Liste der zu entladenden Szenen

    private int currentLoadIndex = 0;
    private int currentUnloadIndex = 0;

    void Update()
    {
        // Prüft, ob die Taste 'P' gedrückt wird
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextScene();
            
        }

        // Prüft, ob die Taste 'U' gedrückt wird
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (currentLoadIndex < scenesToLoad.Count)
        {
            string sceneName = scenesToLoad[currentLoadIndex];
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                Debug.Log("Loaded scene: " + sceneName);
                currentLoadIndex++;
            }
            else
            {
                Debug.LogWarning("Scene " + sceneName + " is already loaded.");
            }
        }
        else
        {
            Debug.Log("All scenes in the load list have been loaded.");
        }
    }

    private void UnloadNextScene()
    {
        if (currentUnloadIndex < scenesToUnload.Count)
        {
            string sceneName = scenesToUnload[currentUnloadIndex];
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
                Debug.Log("Unloaded scene: " + sceneName);
                currentUnloadIndex++;
            }
            else
            {
                Debug.LogWarning("Scene " + sceneName + " is not loaded.");
            }
        }
        else
        {
            Debug.Log("All scenes in the unload list have been unloaded.");
        }
    }
}
