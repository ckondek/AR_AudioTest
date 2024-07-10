using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDebugLoader : MonoBehaviour
{
    public string sceneToLoad = "Scene2"; // Name der zu ladenden Szene
    public string sceneToUnload = "Scene1"; // Name der zu entladenden Szene

    void Update()
    {
        // Prüft, ob die Taste 'P' gedrückt wird
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadScene(sceneToLoad);
        }

        // Prüft, ob die Taste 'U' gedrückt wird
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadScene(sceneToUnload);
        }
    }

    private void LoadScene(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Debug.Log("Loaded scene: " + sceneName);
        }
        else
        {
            Debug.LogWarning("Scene " + sceneName + " is already loaded.");
        }
    }

    private void UnloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log("Unloaded scene: " + sceneName);
        }
        else
        {
            Debug.LogWarning("Scene " + sceneName + " is not loaded.");
        }
    }
}
