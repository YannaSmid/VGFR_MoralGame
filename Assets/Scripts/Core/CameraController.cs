using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private bool isCameraLocked = false;
    private Vector3 lockedPosition;

    private void Update()
    {
        if (isCameraLocked)
        {
            // Keep the camera in the locked position
            transform.position = lockedPosition;

            //Follow player
            //transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
            //lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
        }
        else
        {
            // Room camera movement logic
            transform.position = Vector3.SmoothDamp(
                transform.position,
                new Vector3(currentPosX, transform.position.y, transform.position.z),
                ref velocity,
                speed
            );
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        if (_newRoom == null)
        {
            Debug.LogError("New room is null. Cannot move camera.");
            return;
        }

        if (!isCameraLocked)
        {
            currentPosX = _newRoom.position.x;
        }
    }

    public void LockCameraPosition()
    {
        isCameraLocked = true;
        lockedPosition = transform.position; // Save current position
    }

    public void UnlockCameraPosition()
    {
        isCameraLocked = false;
    }
}
