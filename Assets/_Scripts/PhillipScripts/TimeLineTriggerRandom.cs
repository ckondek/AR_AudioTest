using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic; // For using List and Random

public class TimeLineTriggerRandom : MonoBehaviour
{
    public PlayableDirector[] playableDirectors; // Array to hold multiple PlayableDirectors
    private List<int> shuffledIndices; // List to hold shuffled indices
    private int currentTimelineIndex = 0; // Index to track the current timeline in the shuffled order
    private bool isPlaying = false; // Flag to track if a timeline is currently playing
    private int lastPlayedIndex = -1; // To track the last played director and avoid repeating

    void Start()
    {
        // Stop all directors initially
        foreach (PlayableDirector pd in playableDirectors)
        {
            pd.Stop();
        }

        // Initialize the shuffled indices
        ShufflePlayableDirectors();
    }

    private void ShufflePlayableDirectors()
    {
        // Create a list of indices
        shuffledIndices = new List<int>();
        for (int i = 0; i < playableDirectors.Length; i++)
        {
            shuffledIndices.Add(i);
        }

        // Shuffle the list
        for (int i = 0; i < shuffledIndices.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledIndices.Count);
            int temp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[randomIndex];
            shuffledIndices[randomIndex] = temp;
        }

        currentTimelineIndex = 0; // Reset the index to the start of the shuffled list
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !isPlaying)
        {
            PlayNextTimeline();
        }
    }

    private void PlayNextTimeline()
    {
        if (playableDirectors.Length == 0) return;
        if (isPlaying) return; // If already playing, don't start a new one

        // Ensure we don't repeat the last played director
        int nextIndex = shuffledIndices[currentTimelineIndex];
        while (nextIndex == lastPlayedIndex && playableDirectors.Length > 1)
        {
            // Pick a different director if the next one is the same as the last played
            currentTimelineIndex = (currentTimelineIndex + 1) % shuffledIndices.Count;
            nextIndex = shuffledIndices[currentTimelineIndex];
        }

        lastPlayedIndex = nextIndex; // Set the last played index

        // Get the next playable director based on the shuffled order
        PlayableDirector currentDirector = playableDirectors[nextIndex];

        // Set up event listener for when the director finishes playing
        currentDirector.stopped += OnPlayableDirectorStopped;
        currentDirector.Play(); // Play the selected timeline
        isPlaying = true; // Set the playing flag to true
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        director.stopped -= OnPlayableDirectorStopped;

        // Move to the next timeline in the shuffled order
        currentTimelineIndex = (currentTimelineIndex + 1) % shuffledIndices.Count;

        // If we've cycled through all the directors, reshuffle the list for the next cycle
        if (currentTimelineIndex == 0)
        {
            ShufflePlayableDirectors();
        }

        isPlaying = false; // Reset the playing flag
    }
}
