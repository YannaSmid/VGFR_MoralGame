using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private PlayerMovement movement;
    public bool startDialogue = false;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        
    }

    void Update(){
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            canInteract();

        if (Input.GetKeyUp(KeyCode.Q)){
            Debug.Log("stop interacting");
            cannotInteract();
        }

    }

    void canInteract(){
        startDialogue = true;
    }

    public void cannotInteract(){
        startDialogue = false;
    }

    
}
