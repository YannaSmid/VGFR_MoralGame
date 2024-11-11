using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private CoinManager coinManager;

    private void Start()
    {
        // Find CoinManager in scene
        coinManager = FindObjectOfType<CoinManager>();
        if (coinManager == null)
        {
            Debug.LogError("CoinManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find CoinPickupManager
            CoinPickup coinManagerScript = FindObjectOfType<CoinPickup>();
            if (coinManagerScript != null)
            {
                // Handle coin
                coinManagerScript.HandleCoinPickup(gameObject);
            }
            else
            {
                Debug.LogError("CoinPickupManager not found in the scene!");
            }

            // Increment coin count using CoinManager
            coinManager.AddCoins(1);
        }
    }
}
