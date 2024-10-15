using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI Slider

    private void Start()
    {
        // Set the initial volume based on the slider value
        volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 1f); // Load saved volume or default to 1
        SetVolume(volumeSlider.value); // Apply the initial volume

        // Add listener to handle volume changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // Set the volume for all AudioSources in the scene
        AudioListener.volume = volume; // Adjust global volume

        // Optionally, save the volume setting
        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();
    }
}
