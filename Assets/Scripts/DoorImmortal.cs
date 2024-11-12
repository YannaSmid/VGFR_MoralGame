using UnityEngine;

public class DoorImmortal : MonoBehaviour
{
    [Header("Room References")]
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Immortal Item Tracker")]
    [SerializeField] private ImmortalItem immortalItem; // Reference to ImmortalItem
    [SerializeField] private BoxCollider2D doorCollider; // Reference to blocking collider

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();

        // Ensure blocking collider is assigned
        if (doorCollider == null)
        {
            doorCollider = GetComponent<BoxCollider2D>();
        }

        if (immortalItem == null)
        {
            Debug.LogError("ImmortalItem is not assigned! Assign it in the Inspector.");
        }
    }

    private void Update()
    {
        // Disable door blocker based on whether ImmortalItem has been picked up
        if (immortalItem != null)
        {
            doorCollider.enabled = !immortalItem.pickedUp; // Block door if item not picked up
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && immortalItem != null && immortalItem.pickedUp)
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
        else if (!immortalItem.pickedUp)
        {
            Debug.Log("You must pick up the Immortal Item before opening this door.");
        }
    }
}
