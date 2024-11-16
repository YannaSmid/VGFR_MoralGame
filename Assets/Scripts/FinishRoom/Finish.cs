using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private Door door; // Reference to Door script
    [SerializeField] private float boundaryOffset = 1.5f; // Offset to the right of the door
    [SerializeField] private float finishDelay = 3f; // Delay before transitioning to the finish scene

    private Transform player;
    private bool hasTriggeredFinish = false;

    private void Awake()
    {
        // Find player in scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }

        // Ensure Door reference is set
        if (door == null)
        {
            Debug.LogError("Door script reference is not assigned!");
        }
    }

    private void Update()
    {
        if (player != null && door != null && !hasTriggeredFinish)
        {
            // Calculate boundary position (door's position + offset)
            float doorBoundary = door.transform.position.x + boundaryOffset;

            // Check if player has crossed the boundary
            if (player.position.x > doorBoundary)
            {
                hasTriggeredFinish = true;
                Debug.Log("Player crossed the door boundary. Preparing to finish.");
                StartCoroutine(TriggerFinish());
            }
        }
    }

    private System.Collections.IEnumerator TriggerFinish()
    {
        yield return new WaitForSeconds(finishDelay); // Wait for specified delay
        Debug.Log("Loading the finish scene...");
        SceneManager.LoadScene("FinishScene"); // Load finish scene
    }
}
