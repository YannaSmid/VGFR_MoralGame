using UnityEngine;
using System.Collections;

public class FireChoice : MonoBehaviour
{
    [Header("Fire GameObject")]
    [SerializeField] private GameObject fireObject; // Reference to fire

    [Header("Dialogue GameObject")]
    [SerializeField] private GameObject dialogueObject; // Reference to dialogue

    [Header("Visual Cue GameObject")]
    [SerializeField] private GameObject visualCueObject; // Reference to NPC's visual cue

    [Header("NPC Decision")]
    private DialogueTrigger dialogueTrigger;
    public bool decisionMade = false;
    public bool inRange = false;
    public SPUM_Prefabs _prefabs;

    public static bool fireDamageDisabled = false; // Disable fire damage globally

    private bool visualCueTriggered = false; // Tracks if visual cue has been triggered

    [Header("FireDeath GameObject")]
    [SerializeField] private GameObject fireDeath; // FireDeath GameObject that gets enabled when not helping NPC

    [Header("smokeDeath GameObject")]
    [SerializeField] private GameObject smokeDeath; // Smoke gets enabled when NPC has died

    void Start() 
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();

        if (visualCueObject == null)
        {
            Debug.LogError("Visual Cue GameObject is not assigned! Assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (dialogueTrigger == null) return;

        // Check for player decision
        if (dialogueTrigger.choiceIsMade && !decisionMade && inRange)
        {
            ApplyDecision(DialogueManager.GetInstance().selectedChoice);
        }

        // Check if the visual cue has been enabled
        if (visualCueObject != null && visualCueObject.activeSelf && !visualCueTriggered)
        {
            visualCueTriggered = true; // Ensure it only triggers once
            DisableFireDamage();
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
                Debug.Log("Player chose to help the NPC.");
                DisableFire();
                EnableDialogue();
                decisionMade = true; // Prevent further execution
                break;

            case 1: // Player chooses not to help
                Debug.Log("Player chose not to help the NPC.");
                fireDeath.SetActive(true); // Flames show up
                _prefabs.PlayAnimation(2); // NPC dies
                StartCoroutine(EnableWithDelay(1.0f)); // Start the coroutine with a 1-second delay for smoke
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

    private IEnumerator EnableWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for specified delay
        smokeDeath.SetActive(true); // Enable smoke
        Debug.Log("Smokeeeee"); 
    }
}
