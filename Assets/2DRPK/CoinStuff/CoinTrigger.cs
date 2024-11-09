using UnityEngine;
using TMPro; // For TextMeshPro UI

public class CoinTrigger : MonoBehaviour
{
    private static int coinCount = 0;
    private TextMeshProUGUI coinCounterText; // Reference to the Coin Counter UI Text

    private void Start()
    {
        // Find the CoinCount TextMeshPro object
        GameObject coinTextObject = GameObject.Find("Core/UICanvas/CoinCount");
        if (coinTextObject != null)
        {
            coinCounterText = coinTextObject.GetComponent<TextMeshProUGUI>();
        }

        if (coinCounterText == null)
        {
            Debug.LogError("Coin Counter TextMeshPro object not found! Check the hierarchy and ensure the name is correct.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find the CoinPickupManager
            CoinPickup coinManager = FindObjectOfType<CoinPickup>();
            if (coinManager != null)
            {
                // Handle the coin
                coinManager.HandleCoinPickup(gameObject);
            }
            else
            {
                Debug.LogError("CoinPickupManager not found in the scene!");
            }

            // Increment coin count
            coinCount++;

            // Update coin counter UI
            UpdateCoinCounterUI();
        }
    }

    private void UpdateCoinCounterUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = "Coins: " + coinCount.ToString();
        }
        else
        {
            Debug.LogError("Coin Counter TextMeshPro is not assigned or found!");
        }
    }
}
