using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCage : MonoBehaviour
{

    private DialogueTrigger dialogueTrigger;
    

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] leverTexts;
    [SerializeField] private TextAsset[] humanTexts;
    [SerializeField] private TextAsset[] swordTexts;

    private int fileIndex = 0; // first index is the standard dialogue

    public bool pulledLever = false;
    public bool inRange = false;

    [Header("Cages")]
    public GameObject Cage1;
    public GameObject Cage2;

    [Header("Prisoned Triggers")]

    public GameObject NPC;
    public GameObject Weapon;

    // Start is called before the first frame update
    void Start()
    {
        // dialogueManager = GameObject.Find("DialogueManager");
        dialogueTrigger = GetComponent<DialogueTrigger>();
        
        //dialogueTrigger.inkJSON = textFiles[fileIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueTrigger.choiceIsMade && !pulledLever && inRange){
            pulledLever = true;
            PullLever(DialogueManager.GetInstance().selectedChoice);

        }
        // if (pulledLever){
        //     dialogueTrigger.choiceIsMade = true;
        //     PullLever(dialogue.selectedChoice);
        // }
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

    public void PullLever(int choice){

        switch(choice) 
        {
        case 0: // Human freed
      
            Debug.Log("CHOICE 0");
            NPC.GetComponent<DialogueTrigger>().inkJSON = humanTexts[1]; // Thankful dialogue
            Weapon.GetComponent<DialogueTrigger>().inkJSON = swordTexts[1]; // Sword cannot be gained anymore text
            Cage1.SetActive(false);
            break;
        case 1: // Choose sword
          
            Debug.Log("CHOICE 1");
            NPC.GetComponent<DialogueTrigger>().inkJSON = humanTexts[2]; // Human is sad
            Cage2.SetActive(false);
            Weapon.transform.parent.gameObject.GetComponent<PickUpSword>().swordAvailable = true;
            //Weapon.GetComponent<DialogueTrigger>().inkJSON = swordTexts[1]; // Sword cannot be gained anymore text
            break;
        default:
            // code block
            break;
        }

    }
}
