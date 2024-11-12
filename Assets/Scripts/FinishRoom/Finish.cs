using UnityEngine;

public class Finish : MonoBehaviour
{
    [Header("Closed GameObject")]
    [SerializeField] private GameObject closedObject; // Assign "Closed" GameObject

    private bool triggerActivated = false; // Ensure action only triggers once

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggerActivated)
        {
            triggerActivated = true;
            EnableClosedObject();
        }
    }

    private void EnableClosedObject()
    {
        if (closedObject != null)
        {
            closedObject.SetActive(true); // Enable "Closed" GameObject
            Debug.Log("Closed GameObject has been enabled!");
        }
    }
}
