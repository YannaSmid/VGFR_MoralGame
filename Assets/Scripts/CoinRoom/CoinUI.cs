using UnityEngine;
using TMPro; // For TextMeshPro UI

public class CoinUI : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI coinText; // Reference to the coin UI text
    private int coinCount;

    void Start()
    {
        coinCount = 100; // Set an initial value (can be loaded from save data)
        UpdateCoinUI();
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void SetCoinCount(int newAmount)
    {
        coinCount = newAmount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
       // coinText.text = "Coins: " + coinCount.ToString();
    }
}
