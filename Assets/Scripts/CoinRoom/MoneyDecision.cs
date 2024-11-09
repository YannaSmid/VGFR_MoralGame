using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDecision : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset[] moneyDecisionTexts; // 0 = Default, 1 = NoMoney, 2 = YesMoney

    public bool decisionMade = false;
    public bool inRange = false;

    [Header("NPC Reference")]
    public GameObject NPC; // Reference to the NPC GameObject

    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
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
        if (collider.gameObject.tag == "Player")
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
