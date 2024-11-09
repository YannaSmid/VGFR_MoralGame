using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro UI

public class MoneyDecision : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset[] moneyDecisionTexts; // 0 = Default, 1 = GiveMoney, 2 = KeepMoney

    public bool decisionMade = false;
    public bool inRange = false;

    [Header("NPC Reference")]
    public GameObject NPC; // Reference NPC GameObject

    private TextMeshProUGUI coinCounterText; // Reference Coin Counter UI Text
    private static int coinCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();

        // Find CoinCount text
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

    // Update is called once per frame
    void Update()
    {
        if (dialogueTrigger.choiceIsMade && !decisionMade && inRange)
        {
            decisionMade = true;
            ApplyDecision(DialogueManager.GetInstance().selectedChoice);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    public void ApplyDecision(int choice)
    {
        switch (choice)
        {
            case 0: // Player gives all their money
                Debug.Log("CHOICE 0: Gave all money.");
                ResetCoinCount(); // Reset the coin count to 0
                NPC.GetComponent<DialogueTrigger>().inkJSON = moneyDecisionTexts[1]; // Set GiveMoney dialogue
                break;

            case 1: // Player keeps their money
                Debug.Log("CHOICE 1: Kept money.");
                NPC.GetComponent<DialogueTrigger>().inkJSON = moneyDecisionTexts[2]; // Set KeepMoney dialogue
                break;

            default:
                Debug.LogError("Invalid choice received!");
                break;
        }
    }

    private void ResetCoinCount()
    {
        coinCount = 0; // Reset coin counter

        // Update Coin Counter UI
        if (coinCounterText != null)
        {
            coinCounterText.text = "Coins: " + coinCount.ToString();
        }
        else
        {
            Debug.LogError("Coin Counter TextMeshPro is not found or assigned!");
        }
    }

    // Public method to increment the coin count (optional if coins are added during gameplay)
    public void AddCoin(int amount)
    {
        coinCount += amount;

        // Update the UI
        if (coinCounterText != null)
        {
            coinCounterText.text = "Coins: " + coinCount.ToString();
        }
    }
}
