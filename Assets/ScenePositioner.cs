using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ScenePositioner : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager; // Referenz auf den ARTrackedImageManager
    private Vector3 lastTrackedImagePosition; // Letzte Position des getrackten Bildes
    private Quaternion lastTrackedImageRotation;
    private bool imageTracked = false; // Flag, um zu überprüfen, ob das Bild verfolgt wird

    private void OnEnable()
    {
        // Überwachen des trackedImagesChanged Events
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        // Abbestellen des Events
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // Wenn ein neues Bild erkannt wird, speichere die Position
            lastTrackedImagePosition = trackedImage.transform.position;
           lastTrackedImageRotation = trackedImage.transform.rotation;
            imageTracked = true; // Bild wurde erfolgreich verfolgt1
        }
    }

    public void PositionSceneToImage()
    {
        if (imageTracked)
        {
            // Alle GameObjects in der Szene verschieben
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Überspringe den XR Origin und die nicht benötigten Objekte
                if (obj.name != "XR Origin" && obj.activeInHierarchy)
                {
                    obj.transform.position += lastTrackedImagePosition; // Verschiebe die Objekte
                   obj.transform.rotation = lastTrackedImageRotation;
                }
            }

            Debug.Log($"Die Szene wurde an die Position des getrackten Bildes verschoben.");
            imageTracked = false; // Setze das Flag zurück
        }
    }
}
