using UnityEngine;

public class FireChoice: MonoBehaviour
{
    [Header("Fire GameObject")]
    [SerializeField] private GameObject fireObject; // Reference to fire GameObject

    [Header("Dialogue GameObject")]
    [SerializeField] private GameObject dialogueObject; // Reference to dialogue GameObject

    [Header("NPC Decision")]
    private DialogueTrigger dialogueTrigger;
    public bool decisionMade = false;
    public bool inRange = false;

    public static bool fireDamageDisabled = false; // Flag to disable fire damage globally

    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    void Update()
    {
        if (dialogueTrigger == null) return;

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

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log("Player exited NPC range.");
        }
    }

    private void ApplyDecision(int choice)
    {
        switch (choice)
        {
            case 0: // Player chooses to help NPC
                Debug.Log("CHOICE 0: Player chose to help the NPC.");
                DisableFire();
                EnableDialogue();
               // DisableFireDamage()
                decisionMade = true; // Prevent further execution
                break;

            case 1: // Player chooses not to help
                Debug.Log("CHOICE 1: Player chose not to help the NPC.");
                decisionMade = true; // Prevent further execution
                break;

            default:
                Debug.LogError("Invalid choice received!");
                break;
        }
    }

    private void DisableFire()
    {
        if (fireObject != null)
        {
            fireObject.SetActive(false); // Disable fire GameObject
            Debug.Log("Fire has been disabled!");
        }
    }

    private void EnableDialogue()
    {
        if (dialogueObject != null)
        {
            dialogueObject.SetActive(true); // Enable dialogue GameObject
            Debug.Log("Dialogue object has been enabled!");
        }
    }

    private void DisableFireDamage()
    {
        fireDamageDisabled = true; // Disable fire damage
        Debug.Log("Fire damage has been disabled.");
    }
}
