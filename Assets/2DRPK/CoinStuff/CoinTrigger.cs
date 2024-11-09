using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find the CoinPickupManager in the scene
            CoinPickup coinManager = FindObjectOfType<CoinPickup>();
            if (coinManager != null)
            {
                // Notify the CoinPickupManager to handle the coin
                coinManager.HandleCoinPickup(gameObject);
            }
            else
            {
                Debug.LogError("CoinPickupManager not found in the scene!");
            }
        }
    }
}