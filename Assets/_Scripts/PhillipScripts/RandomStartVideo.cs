using UnityEngine;
using UnityEngine.Video;

public class RandomStartVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Check if the VideoPlayer has a valid video clip loaded
        if (videoPlayer.clip != null)
        {
            // Get the duration of the video in seconds
            double videoDuration = videoPlayer.clip.length;

            // Generate a random time between 0 and the video duration
            double randomTime = Random.Range(0f, (float)videoDuration);

            // Set the VideoPlayer to start from the random time
            videoPlayer.time = randomTime;

            // Play the video
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("No video clip is assigned to the VideoPlayer.");
        }
    }
}
