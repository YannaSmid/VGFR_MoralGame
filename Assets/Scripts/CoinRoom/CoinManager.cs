using UnityEngine;
using TMPro; // For TextMeshPro UI

public class CoinManager : MonoBehaviour
{
    private static int coinCount = 0;

    private TextMeshProUGUI coinCounterText; // Reference to Coin Counter UI Text

    private void Start()
    {
        // Find CoinCount UI Text in the hierarchy
        GameObject coinTextObject = GameObject.Find("Core/UICanvas/CoinCount");
        if (coinTextObject != null)
        {
            coinCounterText = coinTextObject.GetComponent<TextMeshProUGUI>();
        }

        if (coinCounterText == null)
        {
            Debug.LogError("Coin Counter TextMeshPro object not found! Check the hierarchy and ensure the name is correct.");
        }

        // Initialize UI
        UpdateCoinCounterUI();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateCoinCounterUI();
    }

    public void ResetCoins()
    {
        coinCount = 0;
        UpdateCoinCounterUI();
    }

    private void UpdateCoinCounterUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = coinCount.ToString();
        }
        else
        {
            Debug.LogError("Coin Counter TextMeshPro is not found or assigned!");
        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
}
