using UnityEngine;

public class DoorImmortal : MonoBehaviour
{
    [Header("Room References")]
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Immortal Item Tracker")]
    [SerializeField] private ImmortalItem immortalItem; // Reference to ImmortalItem

    [Header("Barrier")]
    [SerializeField] private GameObject barrier; // Manually assign the barrier GameObject in the Inspector

    private Transform player; // Reference to the player's Transform
    private bool barrierDisabled = false; // Tracks if the barrier has already been disabled

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

        // Validate ImmortalItem reference
        if (immortalItem == null)
        {
            Debug.LogError("ImmortalItem is not assigned! Assign it in the Inspector.");
        }
    }

    private void Update()
    {
        // Disable the barrier if the immortal item is picked up and it hasn't been disabled yet
        if (immortalItem != null && immortalItem.pickedUp && barrier != null && !barrierDisabled)
        {
            DisableBarrier();
        }

        // Re-enable the barrier if the player moves 1.5f beyond the door and it is currently disabled
        if (player != null && barrier != null && !barrier.activeSelf)
        {
            float doorBoundary = transform.position.x + 1.5f; // Adjust the offset as needed
            if (player.position.x > doorBoundary)
            {
                EnableBarrier();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && immortalItem != null && immortalItem.pickedUp)
        {
            // Move player to next or previous room based on their position
            if (collision.transform.position.x < transform.position.x)
            {
                MoveToRoom(nextRoom);
            }
            else
            {
                MoveToRoom(previousRoom);
            }
        }
        else if (immortalItem != null && !immortalItem.pickedUp)
        {
            Debug.Log("You must pick up the Immortal Item before opening this door.");
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

    private void DisableBarrier()
    {
        if (barrier != null)
        {
            barrier.SetActive(false); // Disable the barrier
            barrierDisabled = true;  // Prevent it from being disabled again
            Debug.Log("Barrier disabled after picking up the Immortal Item.");
        }
    }

    private void EnableBarrier()
    {
        if (barrier != null)
        {
            barrier.SetActive(true); // Re-enable the barrier
            Debug.Log("Barrier re-enabled after passing the door.");
        }
    }
}
