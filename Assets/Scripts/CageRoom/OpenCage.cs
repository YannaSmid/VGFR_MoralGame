using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCage : MonoBehaviour
{

    private DialogueTrigger dialogueTrigger;

    public bool pulledLever = false;
    public bool inRange = false;

    [Header("Cages")]
    public GameObject Cage1;
    public GameObject Cage2;

    // Start is called before the first frame update
    void Start()
    {
        // dialogueManager = GameObject.Find("DialogueManager");
        dialogueTrigger = GetComponent<DialogueTrigger>();
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

    public void PullLever(int choice){

        switch(choice) 
        {
        case 0:
            // code block
            Debug.Log("CHOICE 0");
            break;
        case 1:
            // code block
            Debug.Log("CHOICE 1");
            break;
        default:
            // code block
            break;
        }

    }
}
