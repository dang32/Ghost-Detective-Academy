using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using LitJson;

public class TextBoxManager : MonoBehaviour {
    public ViewSwitch cameraZoom;
	public GameObject textBox;
	public Text screenText;
	public GameObject dialoguePrefab;

	public List<string> textLines = new List<string>();
	public List<string> questionLines = new List<string>();
	public List<string> currentChain = new List<string>();
	public List<string> checkChain = new List<string>();
	public WordBankManager wordBank;
	public ListenJsonParser listenJsonParser;
	public QuestionJsonParser questionJsonParser;
	private Button wordButton;
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


    void Start()
    {
        cameraZoom = FindObjectOfType<ViewSwitch>();
        playerHistory = FindObjectOfType<PlayerHistory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        wordBank = FindObjectOfType<WordBankManager>();
//        textBox = GameObject.Find("DialoguePanel");
        listenJsonParser = FindObjectOfType<ListenJsonParser>();
        questionJsonParser = FindObjectOfType<QuestionJsonParser>();

        DisableTextBox();
        playerMovement.frozen = false;
        canvas = GameObject.Find("UICanvas");
        listenJson = listenJsonParser.parseLevelJson("levelOne");
		questionJson = questionJsonParser.parseLevelJson("levelOne");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || !isClicked)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isActive)
        {
//            Debug.Log("currentLine " + currentLine);
//            Debug.Log("endline " + endAtLine);
			// chain
			// if (isChain && !listenClick && !questionClick)
			// {
				// Debug.Log("cL" + currentLine);
				// currentLine += 1;
				// Debug.Log("new cL" + currentLine);
				// Debug.Log("eL" + endAtLine);
				// Debug.Log("CC " + currentChain.Count);
				// if (currentLine <= endAtLine)
                // {
                	// Debug.Log("here");
					// screenText.text = questionLines[currentLine];
                // }
                // else if (currentChain.Count == 0)
                // {
                	// Debug.Log("sdjfhjakfhkjashdka");
                	// for (int i = 0; i < checkChain.Count; i++)
                	// {
                		// playerHistory.removeWord(checkChain[i]);
                	// }
					// for (int i = 0; i < playerHistory.wordList.Count; i++)
                	// {
                		// Debug.Log(playerHistory.wordList[i]);
                	// }
                	// isChain = false;
					// DisableTextBox();
					// wordBank.DisableWordBank();
					// isActive=false;
					// return;
                // }
                // else
                // {
                	// screenText.text = "Well? Is that all?";
                	// wordBank.EnableWordBank();
                	// isActive = true;
                	// return;
                // }
			// }
			// else if (!isChain && !listenClick && !questionClick)
			// {
				// Debug.Log("Sonnnnn");
				// DisableTextBox();
				// wordBank.DisableWordBank();
				// isActive=false;
				// return;
			// }

            //listening
            if (listenClick)
            {
				currentLine += 1;
                if(currentLine <= endAtLine)
                {
                    screenText.text = textLines[currentLine];
                }
                else 
                {
					isActive = false;
                    DisableTextBox();
                    wordBank.DisableWordBank();
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
                    DisableTextBox();
                    wordBank.DisableWordBank();
                    isActive = false;
                    return;
                }
            }

        }
    }


    // Use this for initialization
    public void onListenClick() {
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
		for (int i = 0; i < listenJson.Count; i++)
    	{
    		if (listenJson[i].name == npcName)
    		{
				for (int j = 0; j < listenJson[i].wordBank.Count; j++)
				{
					playerHistory.addWord(listenJson[i].wordBank[j]);
				}
			}
		}
		if (endAtLine == 0){
			endAtLine = textLines.Count - 1;
		}
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
		for (int i = 0; i < questionJson.Count; i++)
    	{
    		if (questionJson[i].name == npcName)
    		{
    			screenText.text = questionJson[i].start;
			}
		}
    }

	public void onWordClick(string word){
		for (int i = 0; i < questionJson.Count; i++)
    	{
    		if (questionJson[i].name == npcName)
    		{
    			List<string> text;
    			questionLines.Clear();
    			if (questionJson[i].contra.TryGetValue(word, out text))
    			{
					for (int j = 0; j < text.Count; j++)
					{
						questionLines.Add(text[j]);
					}
				}
				else if (questionJson[i].flavor.TryGetValue(word, out text))
				{
					for (int j = 0; j < text.Count; j++)
					{
						questionLines.Add(text[j]);
					}
				}
				else 
				{
					questionLines.Add(questionJson[i].defaultText);
				}
			}
		}
		if (endAtLine == 0){
			Debug.Log(questionLines.Count);
			endAtLine = questionLines.Count - 1;
			Debug.Log(endAtLine);
		}
		currentLine = 0;
		screenText.text = questionLines[currentLine];
		isClicked = true;
		stopPlayerMovement = true;
		questionClick = true;
		listenClick = false;
	}

	// private bool checkChains(List<List<string>> chainsList, string word){
		// if (currentChain.Count == 0 && isChain == false){
			// Debug.Log("line 249");
			// for (int i = 0; i < chainsList.Count; i++){
				// Debug.Log("line 251");
				// if (chainsList[i][0] == word){
					// isChain = true;
					// currentChain = chainsList[i];
					// checkChain = chainsList[i];
					// return true;
				// }
			// }
		// }
		// else if (isChain)
		// {
			// return true;
		// }
		// currentChain.Clear();
		// isChain = false;
		// return false;
	// }

	public void EnableTextBox(){
		isActive = true;
        playerMovement.frozen = false;
		textLines.Clear();
		questionLines.Clear();
		endAtLine = 0;
		currentLine = 0;
	}

	public void DisableTextBox(){
		textBox.SetActive(false);
		isActive = false;
        cameraZoom.isTalking = false;
        questionClick = false;
        playerMovement.frozen = true;
		endAtLine = 0;
		currentLine = 0;
		listenClick = false;
        textLines.Clear();
        questionLines.Clear();
    }
}