using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveFood : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] humanTexts;

    public bool pickedUp = false;
    public bool foodEaten = false; // choice has been made
    [SerializeField] private AudioClip eatingSound;
    public bool inRange = false;

    [Header("Prologue Options")]
    [Tooltip("Set true if there is a prologue.")] public bool prologueAvailable = false;
    private bool showPrologue = false;
    public GameObject NPCtrigger;

    // Start is called before the first frame update
    void Start()
    {
        //dialogueTrigger = NPC.GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueTrigger.choiceIsMade && !foodEaten && inRange){
            foodEaten = true;
            GiveAway(DialogueManager.GetInstance().selectedChoice);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        { 
            inRange = false;
        }
        if (collider.gameObject.tag == "Player" && dialogueTrigger.choiceIsMade && prologueAvailable && !showPrologue)
        { 
            showPrologue = true;
            StartCoroutine(ActivatePrologue());
        }
    }

    public void GiveAway(int choice){
        // Only do this if you want still want ot be able to interact with object after choice is made
        switch(choice) 
        {
        case 0: // Keep food
            Debug.Log("CHOICE 0");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[2]; // Human is starving
            SoundManager.instance.PlaySound(eatingSound);
            GameObject player = GameObject.FindWithTag("Player"); // Find player
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.startingHealth += 1f;
                    playerHealth.AddHealth(playerHealth.startingHealth); // Add 1 heart to player's health
                    
                    Debug.Log("Player gained 1 heart!");
                }
                else
                {
                    Debug.LogError("Health component is missing on the Player GameObject!");
                }
             }
             break;
        case 1: // Give Food
          
            Debug.Log("CHOICE 1");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[3]; // Human is happy
            //StartCoroutine(ActivatePrologue());
            break;
        case 2: // activate choice dialogue if applicable
            this.GetComponent<DialogueTrigger>().inkJSON = humanTexts[1];
            break;
        default:
            // code block
            break;
        }

    }

    private IEnumerator ActivatePrologue() 
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Prologue");
        NPCtrigger.SetActive(true);
       
    }
}
