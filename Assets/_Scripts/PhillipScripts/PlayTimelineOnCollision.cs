using UnityEngine;
using UnityEngine.Playables;

public class PlayAndStopTimelineOnCollision : MonoBehaviour
{
    public PlayableDirector timeline; // Assign the timeline in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (timeline != null)
            {
                timeline.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (timeline != null)
            {
                timeline.Stop();
            }
        }
    }
}
