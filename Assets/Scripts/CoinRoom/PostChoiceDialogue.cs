using UnityEngine;

public class PostChoiceDialogue : MonoBehaviour
{
    [Header("Dialogue Trigger Reference")]
    [SerializeField] private DialogueTrigger dialogueTrigger;

    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset defaultDialogue;  // The default dialogue JSON
    [SerializeField] private TextAsset thankYouDialogue; // JSON for gratitude dialogue
    [SerializeField] private TextAsset sadDialogue;      // JSON for sadness dialogue

    private bool followUpTriggered = false;

    void Start()
    {
        // If the DialogueTrigger is not assigned, try to find it automatically
        if (dialogueTrigger == null)
        {
            dialogueTrigger = GetComponent<DialogueTrigger>();
        }

        if (dialogueTrigger == null)
        {
            Debug.LogError("DialogueTrigger not assigned or found! Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        // Wait for the initial choice to be made
        if (dialogueTrigger != null && dialogueTrigger.choiceIsMade && !followUpTriggered)
        {
            followUpTriggered = true;

            // Change the dialogue based on the choice made
            if (dialogueTrigger.selectedChoice == 0) // Player said "Yes"
            {
                dialogueTrigger.inkJSON = thankYouDialogue;
            }
            else if (dialogueTrigger.selectedChoice == 1) // Player said "No"
            {
                dialogueTrigger.inkJSON = sadDialogue;
            }

            // Reset the interaction system to allow new dialogue
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        // Reset the DialogueTrigger state
        dialogueTrigger.choiceIsMade = false;
        dialogueTrigger.selectedChoice = -1; // Reset the selected choice
        dialogueTrigger.choiceIsMade = false;

        // Optionally add a delay before the new interaction is allowed
        Invoke(nameof(EnableInteraction), 0.5f);
    }

    private void EnableInteraction()
    {
        // Allow the player to interact again
        dialogueTrigger.enabled = true;
        Debug.Log("New interaction is now available.");
    }
}
