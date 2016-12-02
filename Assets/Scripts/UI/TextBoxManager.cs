using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;

public class TextBoxManager : MonoBehaviour {
    public ViewSwitch cameraZoom;
	public GameObject textBox;
    public GameObject listenButton;
    public GameObject questionButton;
	public Text screenText;
    public GameObject dialoguePrefab;
    public string defaultText;
    string questionStartText;
    public List<string> textLines = new List<string>();
	public List<string> questionLines = new List<string>();
	public List<string> currentChain = new List<string>();
	public List<string> checkChain = new List<string>();
	public WordBankManager wordBank;
	public ListenJsonParser listenJsonParser;
	public QuestionJsonParser questionJsonParser;
	private GameObject canvas;
	public List<parseListenJson> listenJson = new List<parseListenJson>();
	private List<parseQuestionJson> questionJson = new List<parseQuestionJson>();
	public string npcName; 

	public int currentLine;
	public int endAtLine;


    public PlayerMovement playerMovement;
    public PlayerHistory playerHistory;

    public bool isClicked;
	public bool isActive;
	public bool stopPlayerMovement;
	private bool listenClick;
	private bool questionClick;
	public bool isChain;
    public int placeInChain; 
    public GameObject arrow;
    public bool initalPlayerInput;
    public GameObject noteButton;
    public CanvasGroup noteButtons;
    public int currentLevel;
    public GameObject exclamationMark;
    int currentScene;
    public NextLevelText nextLevelText;
    void Start()
    {
        cameraZoom = FindObjectOfType<ViewSwitch>();
        playerHistory = FindObjectOfType<PlayerHistory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        wordBank = FindObjectOfType<WordBankManager>();
//        textBox = GameObject.Find("DialoguePanel");
        listenJsonParser = FindObjectOfType<ListenJsonParser>();
        questionJsonParser = FindObjectOfType<QuestionJsonParser>();

        playerMovement.frozen = false;
        canvas = GameObject.Find("UICanvas");
        listenJson = listenJsonParser.parseLevelJson("levelOne");
		questionJson = questionJsonParser.parseLevelJson("levelOne");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentScene!=SceneManager.GetActiveScene().buildIndex)
        {
            NewSceneLoaded();
        }
        currentScene = SceneManager.GetActiveScene().buildIndex;
        //sets next arrow in ui based on wether there's another line
        if (currentLine <= endAtLine)
            arrow.SetActive(true);
        else {
            arrow.SetActive(false);
            if (questionClick)
                wordBank.EnableWordBank();
            exclamationMark.SetActive(false);
        }

        //changes next arrow to back arrow
        if (currentLine == endAtLine&&listenClick)
        {
            arrow.transform.GetChild(0).GetComponent<Text>().text="↩";
        }


        //sets initalPlayerInput==false if the player isn't holding input and is talking to character, true if walking 
        if (!Input.anyKey && isActive)
        {
            initalPlayerInput = false;
        }
        else if(!isActive&& Input.anyKey)
            initalPlayerInput = true;

        if(!isActive&& (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")))
        {
            wordBank.DisableWordBank();
            noteButton.SetActive(true);
        }

        //detects wasd and removes player from zoomed view
        if ((isActive&&!cameraZoom.transitioning)&&(!initalPlayerInput && (Input.GetKey("w")|| Input.GetKey("s")|| Input.GetKey("a")|| Input.GetKey("d")) ||
            ((isClicked) && (Mathf.Abs(Input.GetAxis("Horizontal")) > .8f || Mathf.Abs(Input.GetAxis("Vertical")) > .8f))))
        {
            DisableTextBox();
        }
        
    }


    // Use this for initialization

   public void  OnArrowClick()
    {

        //listen
        if (listenClick)
        {
            currentLine += 1;
            if (currentLine <= endAtLine)
            {
                screenText.text = textLines[currentLine];
            }
            else
            {
                wordBank.AddWordsToBank();

                listenClick = false;
                endAtLine = -1;
                currentLine = 0;
                textLines.Clear();
                textBox.SetActive(true);
                listenButton.SetActive(true);
                questionButton.SetActive(true);
                wordBank.DisableWordBank();
                screenText.text = defaultText;
                arrow.transform.GetChild(0).GetComponent<Text>().text = "→";

                return;
            }
        }

        //questions
        if (questionClick)
        {
            currentLine += 1;
            if (currentLine <= endAtLine)
            {
                screenText.text = questionLines[currentLine];
            }
            else
            {
                listenClick = false;
                endAtLine = -1;
                currentLine = 0;
                textLines.Clear();
                screenText.text = questionStartText;
                arrow.transform.GetChild(0).GetComponent<Text>().text = "→";
                wordBank.EnableWordBank();
                return;
            }

        }
    }


    public void onListenClick() {

        Debug.Log(listenJson.Count+" "+npcName);
        //scan json and add speaking lines to textLines list
        for (int i = 0; i < listenJson.Count; i++)
    	{
    		if (listenJson[i].name == npcName)
    		{
				for (int j = 0; j < listenJson[i].dialogue.Count; j++)
				{
					textLines.Add(listenJson[i].dialogue[j]);
					Debug.Log(listenJson[i].dialogue[j]);
				}
			}
		}

        //scan json and add wordbank words to player history list
		for (int i = 0; i < listenJson.Count; i++)
    	{
    		if (listenJson[i].name == npcName)
    		{
                Debug.Log(listenJson[i].wordBank.Count);
                for (int j = 0; j < listenJson[i].wordBank.Count; j++)
				{
					playerHistory.addWord(listenJson[i].wordBank[j]);
				}
			}
		}
        //resets to main listen/question screen
		endAtLine = textLines.Count - 1;
		currentLine = 0;
		screenText.text = textLines[currentLine];
        isClicked = true;
		stopPlayerMovement = true;
		listenClick = true;
		questionClick = false;
	}

	public void onQuestionClick(){
        questionClick = true;
		wordBank.EnableWordBank();

        //load starting text
		for (int i = 0; i < questionJson.Count; i++)
    	{
    		if (questionJson[i].name == npcName)
    		{
    			screenText.text = questionJson[i].start;
                questionStartText = questionJson[i].start;

            }
		}
    }

	public void onWordClick(string word) {
        wordBank.DisableWordBank();
		for (int i = 0; i < questionJson.Count; i++)
    	{
    		if (questionJson[i].name == npcName)
    		{
    			List<string> text;
    			questionLines.Clear();
                int contraDictIndex=0;

                //go through each contradiction word and see if the played is in a far enough chain for it to activate
                foreach (KeyValuePair<string, List<string> > contraWord in questionJson[i].contra)
                {
                    //placeInChain==1 after they click the first word
                    Debug.Log(contraWord.Key + " == " + word + " " + contraDictIndex+" " + placeInChain);
                    if (contraWord.Key == word && contraDictIndex == placeInChain)
                    {
                        exclamationMark.SetActive(true);
                        placeInChain++;
                        questionJson[i].contra.TryGetValue(word, out text);
                        for (int j = 0; j < text.Count; j++)
                        {
                            questionLines.Add(text[j]);
                        }
                    }
                    contraDictIndex++;
                }

                //number of contradictory words == the place in the contradiction chain
                if(questionJson[i].contra.Count==placeInChain)
                {
                    Debug.Log("THAT'S NEXT LEVEL!");
                    nextLevelText.StartCoroutine("Appear");
                }

                if (questionLines.Count == 0)
                {
                    if (questionJson[i].flavor.TryGetValue(word, out text))
                    {
                        placeInChain = 0;
                        for (int j = 0; j < text.Count; j++)
                        {
                            questionLines.Add(text[j]);
                        }
                    }
                    else
                    {
                        placeInChain = 0;
                        questionLines.Add(questionJson[i].defaultText);
                        exclamationMark.SetActive(false);

                    }
                }
			}
		}
		endAtLine = questionLines.Count - 1;
		currentLine = 0;
        screenText.text = questionLines[currentLine];
		isClicked = true;
		stopPlayerMovement = true;
		questionClick = true;
		listenClick = false;
	}

	public void EnableTextBox(){
        cameraZoom.playerStartPos = playerMovement.gameObject.transform.position;
        cameraZoom.playerStartRot = playerMovement.gameObject.transform.rotation;
        Debug.Log("enabled text box");
		isActive = true;
        playerMovement.frozen = true;
		textLines.Clear();
		questionLines.Clear();
		endAtLine = -1;
		currentLine = 0;
        cameraZoom.StartCoroutine("Zoom");
        noteButton.SetActive(false);
        noteButtons.interactable = true;
        wordBank.DisableWordBank();


    }

    public void DisableTextBox(){
        Debug.Log("disabled text box");
        wordBank.DisableWordBank();
        cameraZoom.StartCoroutine("Overhead");
		textBox.SetActive(false);
		isActive = false;
        cameraZoom.isTalking = false;
        questionClick = false;
		endAtLine = -1;
		currentLine = 0;
		listenClick = false;
        textLines.Clear();
        questionLines.Clear();
        placeInChain = 0;
        noteButton.SetActive(true);
        isClicked = false;
        noteButtons.interactable = false;
        arrow.transform.GetChild(0).GetComponent<Text>().text = "→";

    }

    public void NewSceneLoaded()
    {
        cameraZoom = FindObjectOfType<ViewSwitch>();
        playerHistory = FindObjectOfType<PlayerHistory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.frozen = false;
    }
}