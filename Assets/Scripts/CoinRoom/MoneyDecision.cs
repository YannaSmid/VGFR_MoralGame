using System.Collections;
using UnityEngine;

public class MoneyDecision : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset[] moneyDecisionTexts; // 0 = Default, 1 = GiveMoney, 2 = KeepMoney

    public bool decisionMade = false;
    public bool inRange = false;

    [Header("NPC Reference")]
    public GameObject NPC; // Reference to NPC GameObject

    private CoinManager coinManager;

    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();

        // Find CoinManager in the scene
        coinManager = FindObjectOfType<CoinManager>();
        if (coinManager == null)
        {
            Debug.LogError("CoinManager not found in the scene!");
        }
    }

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
                coinManager.ResetCoins(); // Reset coins using CoinManager
                NPC.GetComponent<DialogueTrigger>().inkJSON = moneyDecisionTexts[1];
                break;

            case 1: // Player keeps their money
                Debug.Log("CHOICE 1: Kept money.");
                NPC.GetComponent<DialogueTrigger>().inkJSON = moneyDecisionTexts[2];
                break;

            default:
                Debug.LogError("Invalid choice received!");
                break;
        }
    }
}
