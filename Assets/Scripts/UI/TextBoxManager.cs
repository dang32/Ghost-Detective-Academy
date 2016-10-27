using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using LitJson;

public class TextBoxManager : MonoBehaviour {
    public ViewSwitch cameraZoom;
	public GameObject textBox;
	public Text screenText;
	public GameObject dialoguePrefab;

	private string listenFile;
	private JsonData listenData;
	private string questionFile;
	private JsonData questionData;
	public List<string> textLines = new List<string>();
	public List<string> questionLines = new List<string>();
	public WordBankManager wordBank;
	private Button wordButton;
	private GameObject canvas;

	public int currentLine;
	public int endAtLine;


    public PlayerMovement playerMovement;
    public PlayerHistory playerHistory;

    public bool isClicked;
	public bool isActive;
	public bool stopPlayerMovement;
	private bool listenClick;
	private bool questionClick;



    void Start()
    {
        cameraZoom = FindObjectOfType<ViewSwitch>();
        playerHistory = FindObjectOfType<PlayerHistory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        wordBank = FindObjectOfType<WordBankManager>();

        DisableTextBox();
        playerMovement.frozen = false;
        canvas = GameObject.Find("UICanvas");
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
            currentLine += 1;
            Debug.Log("currentLine " + currentLine);
            Debug.Log("endline " + endAtLine);

            //listening
            if (listenClick)
            {
                if(currentLine <= endAtLine)
                {
                    screenText.text = textLines[currentLine];
                }
                else 
                {
                    DisableTextBox();
                    wordBank.DisableWordBank();
                    isActive = false;
                    return;
                }
            }

            //questions
            if (questionClick)
            {
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


            /*
            if (currentLine > endAtLine && listenClick == true)
            {
                listenFile = File.ReadAllText(Application.dataPath + "/Text/ListenJSON.json");
                listenData = JsonMapper.ToObject(listenFile);
                for (int i = 0; i < listenData["levelOne"][0]["wordBank"].Count; i++)
                {
                    playerHistory.addWord(listenData["levelOne"][0]["wordBank"][i].ToString());
                    Debug.Log(listenData["levelOne"][0]["wordBank"][i].ToString());
                }
                Debug.Log("playerHistory.wordList[0] "+playerHistory.wordList[0]);
                DisableTextBox();
                Debug.Log("currentLine "+currentLine);
                Debug.Log("end at line: "+endAtLine);
                return;
            }
            else if (currentLine > endAtLine && questionClick)
            {
                DisableTextBox();
                wordBank.DisableWordBank();
                return;
            }
            if (listenClick)
            {
                screenText.text = textLines[currentLine];
            }
            else if (questionClick)
            {
                screenText.text = questionLines[currentLine];
            }
            */
        }
    }


    // Use this for initialization
    public void onListenClick() {
		listenFile = File.ReadAllText(Application.dataPath + "/Text/ListenJSON.json");
		listenData = JsonMapper.ToObject(listenFile);
		for (int i = 0; i < listenData["levelOne"][0]["dialogue"].Count; i++)
		{
			textLines.Add((listenData["levelOne"][0]["dialogue"][i]).ToString());
			Debug.Log(listenData["levelOne"][0]["dialogue"][i]);
		}

		if (endAtLine == 0){
			endAtLine = textLines.Count - 1;
		}
		screenText.text = textLines[currentLine];
		isClicked = true;
		stopPlayerMovement = true;
		listenClick = true;
		if(isActive){
			//EnableTextBox();
		}
		else{
			//DisableTextBox();
		}
	}

	public void onQuestionClick(){
        questionClick = true;
        playerHistory.addWord("tomatoes");
        playerHistory.addWord("sunny");
        playerHistory.addWord("umbrella");
		wordBank.EnableWordBank();
        screenText.text = "A question? For a clown?";
        //>:(
        //		questionFile = File.ReadAllText(Application.dataPath + "/Text/QuestionJSON.json");
        //		questionData = JsonMapper.ToObject(questionFile);
        //		for (int i = 0; i < jsonData["levelOne"][0].Count; i++)
        //		{
        //			if (jsonData["levelOne"][0][i] == "")
        //			{
        //				textLines.Add((jsonData["levelOne"][0][i].ToString()));
        //			}
        //		}
    }

	public void onWordClick(string word){
		questionFile = File.ReadAllText(Application.dataPath + "/Text/QuestionJSON.json");
		questionData = JsonMapper.ToObject(questionFile);
//		for (int j =0; j < questionData["levelOne"].Count; j++)
//		{
//			Debug.Log(questionData["levelOne"][j].ToString());
			if (word=="tomatoes")
			{
				for (int i = 0; i < questionData["levelOne"][0][word].Count; i++)
				{
					questionLines.Add((questionData["levelOne"][0][word][i]).ToString());
					Debug.Log(questionData["levelOne"][0][word][i]);
					Debug.Log(questionLines);
				}
			}
            else
        {
            questionLines.Add("flavor text...");
        }
//		}
		if (endAtLine == 0){
			endAtLine = questionLines.Count - 1;
		}
		currentLine = 0;
		screenText.text = questionLines[currentLine];
		isClicked = true;
		stopPlayerMovement = true;
		questionClick = true;
		if(isActive){
			EnableTextBox();
		}
		else{
			DisableTextBox();
		}
	}

	
	public void EnableTextBox(){
		//textBox.SetActive(true);
		isActive = true;
       // cameraZoom.isTalking = isActive;
       // if (stopPlayerMovement)
		//{
            playerMovement.frozen = false;
		//}
	}

	public void DisableTextBox(){
		textBox.SetActive(false);
		isActive = false;
        cameraZoom.isTalking = false;
        questionClick = false;
        playerMovement.frozen = true;
		stopPlayerMovement = false;
		endAtLine = 0;
		currentLine = 0;
		listenClick = false;
        textLines.Clear();
        questionLines.Clear();
//		textBox = (GameObject)Instantiate(dialoguePrefab);
//		textBox.transform.SetParent(canvas.transform, false);
//		this.textBox = textBox;
//		GameObject button = GameObject.Find("ListenButton");
//		button.GetComponent<Button>().onClick.AddListener(delegate { onListenClick();});
//		button = GameObject.Find("QuestionButton");
//		button.GetComponent<Button>().onClick.AddListener(delegate { onQuestionClick();});
    }
}