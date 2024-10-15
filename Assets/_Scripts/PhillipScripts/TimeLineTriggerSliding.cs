using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;

public class TimeLineTriggerSliding : MonoBehaviour
{
    public PlayableDirector[] playableDirectors; // Array to hold multiple PlayableDirectors
    public AudioClip[] audioClips; // Array to hold audio clips
    public AudioSource audioSource; // AudioSource component to play the clips
    public float audioTriggerDelay = 8f; // Time delay before playing the next audio clip

    private int currentTimelineIndex = 0; // Index to track the current timeline
    private bool isPlaying = false; // Flag to track if a timeline is currently playing

    private List<int> shuffledAudioIndices; // List to hold shuffled indices for audio clips
    private int currentAudioIndex = 0; // Index to track the current audio clip
    private int lastPlayedAudioIndex = -1; // To track the last played audio clip
    private bool audioPlayed = false; // Flag to ensure the audio is only played once per trigger

    void Start()
    {
        foreach (PlayableDirector pd in playableDirectors)
        {
            pd.Stop();
        }

        ShuffleAudioClips(); // Shuffle the audio clips at the start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !isPlaying && !audioPlayed)
        {
            PlayCurrentTimeline();
            StartCoroutine(PlayRandomAudioClipWithDelay()); // Play the audio with a delay
        }
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

    // Shuffle the audio clips to ensure a random order
    private void ShuffleAudioClips()
    {
        shuffledAudioIndices = new List<int>();
        for (int i = 0; i < audioClips.Length; i++)
        {
            shuffledAudioIndices.Add(i);
        }

        // Shuffle the list
        for (int i = 0; i < shuffledAudioIndices.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledAudioIndices.Count);
            int temp = shuffledAudioIndices[i];
            shuffledAudioIndices[i] = shuffledAudioIndices[randomIndex];
            shuffledAudioIndices[randomIndex] = temp;
        }

        currentAudioIndex = 0; // Reset the audio index to the start of the shuffled list
    }

    // Coroutine to play a random audio clip with a delay
    private IEnumerator PlayRandomAudioClipWithDelay()
    {
        yield return new WaitForSeconds(audioTriggerDelay); // Wait for the defined delay

        if (audioClips.Length == 0 || audioSource == null) yield break; // If no audio clips, exit

        // Ensure no clip is repeated back-to-back
        int nextAudioIndex = shuffledAudioIndices[currentAudioIndex];
        while (nextAudioIndex == lastPlayedAudioIndex && audioClips.Length > 1)
        {
            currentAudioIndex = (currentAudioIndex + 1) % shuffledAudioIndices.Count;
            nextAudioIndex = shuffledAudioIndices[currentAudioIndex];
        }

        // Play the audio clip
        audioSource.clip = audioClips[nextAudioIndex];
        audioSource.Play();

        // Update tracking variables
        lastPlayedAudioIndex = nextAudioIndex;
        currentAudioIndex = (currentAudioIndex + 1) % shuffledAudioIndices.Count;

        // Reshuffle when all clips have been played once
        if (currentAudioIndex == 0)
        {
            ShuffleAudioClips();
        }

        audioPlayed = true; // Set flag to true to ensure audio plays only once per trigger
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset audioPlayed flag so audio can be triggered again next time the player enters
        if (other.CompareTag("MainCamera"))
        {
            audioPlayed = false;
        }
    }
}
