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
            doorCollider.enabled = !decisionTracker.choiceIsMade; // Block the door if decision not made
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Move player to next or previous room based on their position
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }
}
