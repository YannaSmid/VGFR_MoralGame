using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealFireMagic : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] humanTexts;
    public bool tookAction = false; // choice has been made
    public bool inRange = false;
    public SPUM_Prefabs _prefabs;

    [Header("Prologue Options")]
    [Tooltip("Set true if there is a prologue.")] public bool prologueAvailable = false;
    private bool showPrologue = false;
    public GameObject NPCtrigger;
    public NPCFireball npcFireMagic;

    // Start is called before the first frame update
    void Start()
    {
        //dialogueTrigger = NPC.GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueTrigger.choiceIsMade && !tookAction && inRange){
            tookAction = true;
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
        case 0: // Let her live
            Debug.Log("CHOICE 0");
            NPCtrigger.GetComponent<DialogueTrigger>().inkJSON = humanTexts[1]; // 
            //StartCoroutine(ActivatePrologue());
            break;
        case 1: // Kill her
            Debug.Log("CHOICE 1");
            npcFireMagic.enabled = false;
            _prefabs.PlayAnimation(2);
            //StartCoroutine(ActivatePrologue());
            break;
        case 2: // activate choice dialogue if applicable
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
