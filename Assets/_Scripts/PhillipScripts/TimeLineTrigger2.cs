using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger2 : MonoBehaviour
{
    public PlayableDirector[] playableDirectors; // Array to hold multiple PlayableDirectors
    private int currentTimelineIndex = 0; // Index to track the current timeline
    private bool isPlaying = false; // Flag to track if a timeline is currently playing

    void Start()
    {
        foreach (PlayableDirector pd in playableDirectors)
        {
            pd.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !isPlaying)
        {
            PlayCurrentTimeline();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // No need to stop the timeline here; it should play until the end
    }

    private void PlayCurrentTimeline()
    {
        if (playableDirectors.Length == 0) return;

        isPlaying = true; // Set the playing flag to true
        PlayableDirector currentDirector = playableDirectors[currentTimelineIndex];
        currentDirector.stopped += OnPlayableDirectorStopped;
        currentDirector.Play();
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        director.stopped -= OnPlayableDirectorStopped;

        // Increment the timeline index, and loop back to 0 if at the end
        currentTimelineIndex = (currentTimelineIndex + 1) % playableDirectors.Length;
        isPlaying = false; // Reset the playing flag
    }
}
