using UnityEngine;

public class MoneyDecision : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    [Header("NPC Reference")]
    public GameObject NPC; // Reference NPC GameObject

    [Header("Coin Manager")]
    private CoinManager coinManager;

    public bool decisionMade = false;
    public bool inRange = false;

    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();

        // Find CoinManager
        coinManager = FindObjectOfType<CoinManager>();
        if (coinManager == null)
        {
            Debug.LogError("CoinManager not found in the scene!");
        }
    }

    void Update()
    {
        // Check if player made a choice while in range
        if (dialogueTrigger.choiceIsMade && !decisionMade && inRange)
        {
            ApplyDecision(DialogueManager.GetInstance().selectedChoice);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = true;
            Debug.Log("Player entered NPC range.");
        }
    }

    private void ApplyDecision(int choice)
    {
        switch (choice)
        {
            case 0: // Player chooses to give all their money
                Debug.Log("CHOICE 0: Gave all money.");
                coinManager.ResetCoins(); // Reset coins using CoinManager
                decisionMade = true; // Ensure this happens only once
                break;

            case 1: // Player chooses to keep their money
                Debug.Log("CHOICE 1: Kept money.");
                decisionMade = true; // Ensure this happens only once
                break;

            default:
                Debug.LogError("Invalid choice received!");
                break;
        }
    }
}
