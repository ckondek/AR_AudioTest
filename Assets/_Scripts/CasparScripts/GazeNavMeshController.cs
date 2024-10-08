using UnityEngine;
using UnityEngine.AI;

public class GazeNavMeshController : MonoBehaviour
{
    public Camera mainCamera; // The camera from which we cast the gaze
    public GameObject indicatorPrefab; // The indicator that appears on the surface
    public LayerMask surfaceLayer; // The layer representing the surface with a NavMesh
    public float randomMoveRadius = 10f; // Radius for random movement
    public NavMeshAgent[] agents; // Array of all NavMeshAgents
    public bool distanceMode = false; // Set to true for distance-based movement
    public float distanceThreshold = 5f; // Threshold for distance mode

    private GameObject activeIndicator;
    private bool gazeActive = false;

    void Start()
    {
        // Instantiate the indicator, but keep it hidden initially
        activeIndicator = Instantiate(indicatorPrefab);
        activeIndicator.SetActive(false);
    }

    void Update()
    {
        // Switch between gaze-based and distance-based modes
        if (!distanceMode)
        {
            HandleGazeIndicator();
            MoveAgents();
        }
        else
        {
            MoveAgentsDistanceBased();
        }
    }

    void HandleGazeIndicator()
    {
        // Raycast from the center of the camera to check for surface hit
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, surfaceLayer))
        {
            if (!gazeActive)
            {
                gazeActive = true;
                activeIndicator.SetActive(true);
            }
            activeIndicator.transform.position = hit.point; // Move indicator to hit point
        }
        else
        {
            if (gazeActive)
            {
                gazeActive = false;
                activeIndicator.SetActive(false);
            }
        }
    }

    void MoveAgents()
    {
        if (gazeActive)
        {
            // Move agents to the indicator
            Vector3 targetPosition = activeIndicator.transform.position;
            foreach (NavMeshAgent agent in agents)
            {
                agent.SetDestination(targetPosition);
            }
        }
        else
        {
            // Agents wander randomly
            foreach (NavMeshAgent agent in agents)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition(agent.transform.position, randomMoveRadius);
                agent.SetDestination(randomPosition);
            }
        }
    }

    void MoveAgentsDistanceBased()
    {
        float distanceToSurface = GetCameraDistanceToSurface();

        if (distanceToSurface <= distanceThreshold)
        {
            // Move agents to the point closest to the camera on the NavMesh
            Vector3 closestNavMeshPoint = GetClosestNavMeshPoint(mainCamera.transform.position);
            foreach (NavMeshAgent agent in agents)
            {
                agent.SetDestination(closestNavMeshPoint);
            }
        }
        else
        {
            // Agents wander randomly
            foreach (NavMeshAgent agent in agents)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition(agent.transform.position, randomMoveRadius);
                agent.SetDestination(randomPosition);
            }
        }
    }

    float GetCameraDistanceToSurface()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, surfaceLayer))
        {
            return Vector3.Distance(mainCamera.transform.position, hit.point);
        }

        return Mathf.Infinity; // Return a large number if no surface is hit
    }

    Vector3 GetClosestNavMeshPoint(Vector3 origin)
    {
        NavMeshHit navHit;
        NavMesh.SamplePosition(origin, out navHit, Mathf.Infinity, NavMesh.AllAreas);
        return navHit.position;
    }

    Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas);
        return navHit.position;
    }
}
