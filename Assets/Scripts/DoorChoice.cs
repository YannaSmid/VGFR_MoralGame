using UnityEngine;

public class DoorChoice : MonoBehaviour
{
    [Header("Room References")]
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Decision Tracker")]
    [SerializeField] private DialogueTrigger decisionTracker; // Reference to decision tracker
    [SerializeField] private BoxCollider2D doorCollider; // Reference to blocking collider

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();

        // Ensure blocking collider is assigned
        if (doorCollider == null)
        {
            doorCollider = GetComponent<BoxCollider2D>();
        }
    }

    private void Update()
    {
        // Disable door blocker based on whether decision has been made
        if (decisionTracker != null)
        {
            doorCollider.enabled = !decisionTracker.choiceIsMade; // Block door if decision not made
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < transform.position.x)
            {
                MoveToRoom(nextRoom, previousRoom);
            }
            else
            {
                MoveToRoom(previousRoom, nextRoom);
            }
        }
    }

    private void MoveToRoom(Transform targetRoom, Transform otherRoom)
    {
        cam.MoveToNewRoom(targetRoom);
        targetRoom.GetComponent<Room>().ActivateRoom(true);
        otherRoom.GetComponent<Room>().ActivateRoom(false);
    }
}
