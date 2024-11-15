using UnityEngine;

public class DoorChoice : MonoBehaviour
{
    [Header("Room References")]
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Barrier")]
    [SerializeField] private GameObject barrier; // Assign the barrier GameObject in the Inspector

    [Header("Decision Tracker")]
    [SerializeField] private DialogueTrigger decisionTracker; // Reference to the decision tracker

    private Transform player;  // Reference to the player’s transform
    private bool decisionHandled = false; // Tracks if the barrier was already handled

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();

        // Ensure the barrier starts enabled
        if (barrier != null)
        {
            barrier.SetActive(true);
        }

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    private void Update()
    {
        // Disable the barrier if the decision is made
        if (decisionTracker != null && decisionTracker.choiceIsMade && !decisionHandled)
        {
            HandleDecision();
        }

        // Re-enable the barrier if the player has moved past the door
        if (player != null && barrier != null && !barrier.activeSelf)
        {
            float doorBoundary = transform.position.x + 1.5f; // Adjust the offset as needed
            if (player.position.x > doorBoundary)
            {
                ReEnableBarrier();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < transform.position.x)
            {
                MoveToRoom(nextRoom);
            }
            else
            {
                MoveToRoom(previousRoom);
            }
        }
    }

    private void HandleDecision()
    {
        if (barrier != null)
        {
            barrier.SetActive(false); // Disable the barrier
            decisionHandled = true;  // Mark the decision as handled
            Debug.Log("Barrier disabled after decision.");
        }
    }

    private void ReEnableBarrier()
    {
        if (barrier != null)
        {
            barrier.SetActive(true); // Re-enable the barrier
            Debug.Log("Barrier re-enabled after passing the door.");
        }
    }

    private void MoveToRoom(Transform targetRoom)
    {
        // Move the camera and activate the target room
        cam.MoveToNewRoom(targetRoom);
        targetRoom.GetComponent<Room>().ActivateRoom(true);

        // Deactivate the other room
        if (targetRoom == nextRoom)
        {
            previousRoom.GetComponent<Room>().ActivateRoom(false);
        }
        else
        {
            nextRoom.GetComponent<Room>().ActivateRoom(false);
        }
    }
}
