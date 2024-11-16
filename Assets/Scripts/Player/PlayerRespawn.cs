using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform startingPosition;
    public Transform currentCheckpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform; // Update current checkpoint
            Debug.Log("Checkpoint updated: " + currentCheckpoint.name);

            // Only trigger Animator if it exists
            Animator checkpointAnimator = collision.GetComponent<Animator>();
            if (checkpointAnimator != null)
            {
                checkpointAnimator.SetTrigger("activate");
            }
        }
    }

    public Transform GetRespawnPosition()
    {
        if (currentCheckpoint != null)
        {
            Debug.Log("Returning checkpoint position: " + currentCheckpoint.name);
            return currentCheckpoint;
        }
        else
        {
            Debug.LogWarning("No checkpoint found. Falling back to starting position.");
            return startingPosition;
        }
    }
}
