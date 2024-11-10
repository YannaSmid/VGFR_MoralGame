using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeShoes : MonoBehaviour
{
public DialogueTrigger dialogueTrigger;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] humanTexts;
    public bool inRange = false;
    private bool shoesDone = false;

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
        if (dialogueTrigger.choiceIsMade && inRange && !shoesDone){
            shoesDone = true;
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
        case 0: // Steal shoes
            Debug.Log("CHOICE 0");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[1]; // Human is starving
            //StartCoroutine(ActivatePrologue());
            break;
        case 1: // Let them keep the shoes
          
            Debug.Log("CHOICE 1");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[2]; // Human is happy
            //StartCoroutine(ActivatePrologue());
            break;
        case 2: // activate choice dialogue if applicable
            this.GetComponent<DialogueTrigger>().inkJSON = humanTexts[0];
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
