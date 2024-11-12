using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    public static Transform lastDoorPosition; // Track last door

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Update last door position
            lastDoorPosition = transform;

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

    public static Transform GetLastDoorRoom()
    {
        return lastDoorPosition?.parent; // Return parent room of last door
    }
}
