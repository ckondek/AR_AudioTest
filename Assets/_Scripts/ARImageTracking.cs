using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTracking : MonoBehaviour
{
    // Reference to the AR Tracked Image Manager
    public ARTrackedImageManager trackedImageManager;

    void OnEnable()
    {
        // Subscribe to the tracked images changed event
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // Unsubscribe when disabled
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // This is called when tracked images are updated (added, updated, removed)
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Loop through the newly updated tracked images
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // Get the position from the tracked image
            Vector3 newPosition = trackedImage.transform.position;

            // Adjust the rotation: Keep only rotation on the XZ plane, ignore the Y-axis rotation
            Vector3 newRotationEuler = trackedImage.transform.eulerAngles;
            newRotationEuler.x = 0; // Keep the prefab upright (no tilting forward/backward)
            newRotationEuler.z = 0; // Keep the prefab upright (no tilting sideways)

            // Apply the position and adjusted rotation to the prefab
            trackedImage.transform.position = newPosition;

            // Set the rotation to only rotate along the Y-axis (keeping the object upright)
            trackedImage.transform.rotation = Quaternion.Euler(newRotationEuler);
        }
    }
}
