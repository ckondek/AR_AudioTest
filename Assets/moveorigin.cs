using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MoveOrigin : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;  // Referenz zum ARTrackedImageManager
    public GameObject xrOrigin;  // Der XR Origin (oder die Hauptkamera)
    
    void OnEnable()
    {
        // Abonniere das Event, das aufgerufen wird, wenn ein Bild getrackt wird
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnDisable()
    {
        // Entferne das Event, wenn dieses GameObject deaktiviert wird
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Gehe durch alle neu getrackten Bilder
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // Wenn das Bild erfolgreich getrackt wird, verschiebe den XR Origin zu den Koordinaten des Bildes
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                MoveXROriginToImage(trackedImage);
            }
        }
    }

    private void MoveXROriginToImage(ARTrackedImage trackedImage)
    {
        // Position des getrackten Bildes
        Vector3 imagePosition = trackedImage.transform.position;

        // Optional: Rotation des getrackten Bildes
        Quaternion imageRotation = trackedImage.transform.rotation;

        // Setze die Position und Rotation des XR Origin auf die des Bildes
        xrOrigin.transform.position = imagePosition;
        xrOrigin.transform.rotation = imageRotation;

        Debug.Log($"XR Origin zu {trackedImage.referenceImage.name} bewegt: Position: {imagePosition}, Rotation: {imageRotation}");
    }
}
