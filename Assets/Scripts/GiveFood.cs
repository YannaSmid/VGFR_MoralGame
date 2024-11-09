using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveFood : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] humanTexts;

    private int fileIndex = 0; // first index is the standard dialogue

    public bool pickedUp = false;
    public bool foodEaten = false; // choice has been made
    public bool inRange = false;

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
    }

    public void GiveAway(int choice){
        // Only do this if you want still want ot be able to interact with object after choice is made
        switch(choice) 
        {
        case 0: // Keep food
            Debug.Log("CHOICE 0");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[2]; // Human is starving
            StartCoroutine(ActivatePrologue());
            break;
        case 1: // Give Food
          
            Debug.Log("CHOICE 1");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[3]; // Human is happy
            StartCoroutine(ActivatePrologue());
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
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Prologue");
        NPCtrigger.GetComponent<BoxCollider2D>().enabled = true;
       
    }
}
