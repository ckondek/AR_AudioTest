using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        // Explicitly stop the Playable Director at the start
        playableDirector.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playableDirector.played += OnPlayableDirectorPlayed;
            playableDirector.stopped += OnPlayableDirectorStopped;
            PlayTimelineLoop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playableDirector.played -= OnPlayableDirectorPlayed;
            playableDirector.stopped -= OnPlayableDirectorStopped;
            playableDirector.Stop();
        }
    }

    private void PlayTimelineLoop()
    {
        if (playableDirector.state != PlayState.Playing)
        {
            playableDirector.Play();
        }
    }

    private void OnPlayableDirectorPlayed(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            // Additional logic when the timeline starts (if needed)
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector && playableDirector.state != PlayState.Playing)
        {
            PlayTimelineLoop();
        }
    }
}
