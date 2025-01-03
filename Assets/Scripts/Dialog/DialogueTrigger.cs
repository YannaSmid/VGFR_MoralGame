 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;

    private GameObject player;
    private Interact interact;

    private bool playerInRange;

    public bool choiceIsMade = false;
    public int selectedChoice;

    // [Header("Prologue Options")]
    // public bool prologueAvailable = false; // activate if the dialogue contains a prologue 
    // public bool startPrologue = false;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
        player = GameObject.Find("Player");
        interact = player.GetComponent<Interact>();
    }

    private void Update() 
    {
        if (playerInRange && DialogueManager.GetInstance().choiceMade && !choiceIsMade){
            choiceIsMade = true;
            selectedChoice = DialogueManager.GetInstance().selectedChoice;
       
        }
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !choiceIsMade) 
        {
            visualCue.SetActive(true);
            if (interact.startDialogue) 
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                interact.cannotInteract();
            }
        }
        else 
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        { 
            playerInRange = false;
        }
 
    
    }
}
