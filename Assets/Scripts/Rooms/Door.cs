using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Barrier")]
    [SerializeField] private GameObject barrier; // Manually assign the barrier GameObject in the Inspector

    private Transform player; // Reference to the player's Transform

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }

        // Disable the barrier at the start
        if (barrier != null)
        {
            barrier.SetActive(false);
        }
    }

    private void Update()
    {
        // Re-enable the barrier if the player moves 1.5f beyond the door
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

    private void ReEnableBarrier()
    {
        if (barrier != null)
        {
            barrier.SetActive(true); // Re-enable the barrier
            Debug.Log("Barrier re-enabled after passing the door.");
        }
    }
}
