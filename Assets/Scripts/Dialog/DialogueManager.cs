using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System.Linq;


public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    // private GameObject player;
    // private Interact interact;

    private static DialogueManager instance;

    public bool choicesDisplayed = false;
    public int selectedChoice { get; private set; }
    public bool choiceMade {get; private set; }

    private DecisionLogger logger;
    private float choiceStartTime; // Stores the time when choices are shown

    [Header("Player Settings")]
    public string playerID; // Set this in the Inspector



    private void Awake() 
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        // dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        // inkExternalFunctions = new InkExternalFunctions();
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueIsPlaying = false;
        choiceMade = false;
        dialoguePanel.SetActive(false);
        // player = GameObject.Find("Player");
        // interact = player.GetComponent<Interact>();

        // Get the current scene name as the game version
        string gameVersion = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Initialize the logger
        logger = FindObjectOfType<DecisionLogger>();
        logger.InitializeLogger(gameVersion, playerID);

        Debug.Log($"Logger initialized for PlayerID: {playerID}, Game Version: {gameVersion}");

        // Get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public static DialogueManager GetInstance() 
    {
        return instance;
    }

       // Update is called once per frame
    void Update()
    {
        if (!dialogueIsPlaying){
            return;
        }
        // if (Input.GetKeyDown(KeyCode.Space)){
        //     ContinueStory();
        // }
        if (Input.GetMouseButtonDown(0) && !choicesDisplayed){
            ContinueStory();
        }
    }


    public void EnterDialogueMode(TextAsset inkJSON){
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
        
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // Set text for the current dialogue line
            dialogueText.text = currentStory.Continue();

            // Display choices, if any, for the dialogue line
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ExitDialogueMode() 
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("End of conversation");
        dialogueIsPlaying = false;
        choiceMade = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to ensure the UI can support the number of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError($"More choices were given than the UI can support. Choices count: {currentChoices.Count}");
            return;
        }

        // Record the time when the choices are displayed
        choiceStartTime = Time.time;

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        if (currentChoices.Count > 0)
        {
            choicesDisplayed = true;
        }
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(ClearChoice());
    }


    private IEnumerator SelectFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    
    private IEnumerator ClearChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
    }

    public void MakeChoice(int choiceIndex)
    {
        // Defensive check to ensure the index is within bounds
        if (choiceIndex < 0 || choiceIndex >= currentStory.currentChoices.Count)
        {
            Debug.LogError($"Invalid choice index: {choiceIndex}. Choices count: {currentStory.currentChoices.Count}");
            return; // Exit the method if the index is invalid
        }

        // Process the choice
        string choiceText = currentStory.currentChoices[choiceIndex].text;
        currentStory.ChooseChoiceIndex(choiceIndex);

        // Calculate the time taken to make the decision
        float timeTaken = Time.time - choiceStartTime;

        // Get the current room name
        string roomName = GetCurrentRoomName();

        // Log the player's choice, room name, and time taken
        logger.LogDecision($"Room: {roomName}, Player chose: {choiceText}, Time taken: {timeTaken:F2} seconds");

        selectedChoice = choiceIndex;
        choiceMade = true;
        choicesDisplayed = false;

        ContinueStory();
    }

    private string GetCurrentRoomName()
    {
        // Find the player GameObject
        GameObject player = GameObject.FindWithTag("Player");

        // Use a small overlap area to detect nearby rooms
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(player.transform.position, 1f); // Adjust radius if needed
        foreach (Collider2D collider in nearbyObjects)
        {
            // Check for specific room tags
            if (collider.CompareTag("FireballRoom"))
                return "FireballRoom";
            if (collider.CompareTag("CoinRoom"))
                return "CoinRoom";
            if (collider.CompareTag("FireRoom"))
                return "FireRoom";
        }

        return "Unknown Room"; // Default if no room is detected
    }

    // Helper function to get all PlayerPrefs keys
    private IEnumerable<string> PlayerPrefsKeys(string prefix)
    {
        foreach (var key in PlayerPrefs.GetString("AllKeys", "").Split(','))
        {
            if (key.StartsWith(prefix))
            {
                yield return key;
            }
        }
    }
}
