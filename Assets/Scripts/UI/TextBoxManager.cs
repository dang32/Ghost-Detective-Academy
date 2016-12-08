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
    bool scrollingTextDone=true;
    public int[] characterWantsToSayLevel; //order of scenes
    public int totalContradictions;
    public Transform[] tentExitPositions;
    public int prevScene;
    public AudioSource indoorRain;
    public AudioSource outdoorRain;
    public AudioSource music;
    public bool instructionsDone;
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
        if(Input.GetKeyDown(KeyCode.Space)&&isActive)
        {
            OnArrowClick();
        }
        if(isActive)
        {
            music.volume = Mathf.Lerp(music.volume, .05f, 2 * Time.deltaTime);
        }
        else
        {
            music.volume = Mathf.Lerp(music.volume, .005f, 2 * Time.deltaTime);
        }

        if (SceneManager.GetActiveScene().buildIndex==0)
        {
            outdoorRain.volume= Mathf.Lerp(outdoorRain.volume, .075f, 2 * Time.deltaTime);
        }
        else
        {
            outdoorRain.volume = Mathf.Lerp(outdoorRain.volume, 0, 2 * Time.deltaTime);
        }

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 6 && !isActive)
        {
            indoorRain.volume = Mathf.Lerp(indoorRain.volume, .05f, 2 * Time.deltaTime);
        }
        else {
            indoorRain.volume = Mathf.Lerp(indoorRain.volume, 0, 2 * Time.deltaTime);
        }

        //if (Input.GetKeyDown("l"))
        //{
        //    Debug.Log("THAT'S NEXT LEVEL!");
        //    nextLevelText.StartCoroutine("Appear" ,1);
        //    currentLevel++;
        //    listenJson = listenJsonParser.parseLevelJson("levelTwo");
        //    questionJson = questionJsonParser.parseLevelJson("levelTwo");
        //}

        //if (Input.GetKeyDown("p"))
        //{
        //    SceneManager.LoadScene(6);
        //}
        if (currentScene!=SceneManager.GetActiveScene().buildIndex)
        {
            if(SceneManager.GetActiveScene().buildIndex!=0)
            {
                prevScene = SceneManager.GetActiveScene().buildIndex;
            }
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
            if (SceneManager.GetActiveScene().buildIndex != 6)
            {
                wordBank.DisableWordBank();
                noteButton.SetActive(true);
            }
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
        if(scrollingTextDone)
        currentLine += 1;

        //listen
        if (listenClick)
        {
            if (currentLine <= endAtLine)
            {
                //screenText.text = textLines[currentLine];
                StartCoroutine("TypeText", textLines[currentLine]);
            }
            else
            {
                wordBank.AddWordsToBank();
                characterWantsToSayLevel[SceneManager.GetActiveScene().buildIndex-1] = currentLevel + 1;
                listenClick = false;
                endAtLine = -1;
                currentLine = 0;
                textLines.Clear();
                textBox.SetActive(true);
                listenButton.SetActive(true);
                questionButton.SetActive(true);
                wordBank.DisableWordBank();
                StartCoroutine("TypeText", defaultText);

                //screenText.text = defaultText;
                arrow.transform.GetChild(0).GetComponent<Text>().text = "→";

                return;
            }
        }

        //questions
        if (questionClick)
        {
            if (currentLine <= endAtLine)
            {
                StartCoroutine("TypeText", questionLines[currentLine]);

                //screenText.text = questionLines[currentLine];
            }
            else
            {
                listenClick = false;
                endAtLine = -1;
                currentLine = 0;
                textLines.Clear();
                StartCoroutine("TypeText", questionStartText);

                //screenText.text = questionStartText;
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
                if(currentLevel> characterWantsToSayLevel[SceneManager.GetActiveScene().buildIndex-1])
                {
                    listenJson = listenJsonParser.parseLevelJson("levelOne");
                    questionJson = questionJsonParser.parseLevelJson("levelOne");
                    for (int p = 0; p < listenJson.Count;p++)
                    {
                        if (listenJson[p].name == npcName)
                        {
                            for (int j = 0; j < listenJson[p].wordBank.Count; j++)
                            {
                                playerHistory.addWord(listenJson[p].wordBank[j]);
                            }
                        }
                    }

                    for (int j = 0; j < listenJson[i].dialogue.Count; j++)
                    {
                        List<string> stringsToAdd = SplitStrings(listenJson[i].dialogue[j]);
                        for (int k = 0; k < stringsToAdd.Count; k++)
                        {
                            textLines.Add(stringsToAdd[k]);
                        }

                        Debug.Log(listenJson[i].dialogue[j]);
                    }
                    listenJson = listenJsonParser.parseLevelJson("levelTwo");
                    questionJson = questionJsonParser.parseLevelJson("levelTwo");
                }
				for (int j = 0; j < listenJson[i].dialogue.Count; j++)
				{
                    List < string > stringsToAdd = SplitStrings(listenJson[i].dialogue[j]);
                    for(int k=0;k<stringsToAdd.Count;k++)
                    {
                        textLines.Add(stringsToAdd[k]);
                    }
					
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
        StartCoroutine("TypeText", textLines[currentLine]);

        //screenText.text = textLines[currentLine];
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
                StartCoroutine("TypeText", questionJson[i].start);

              //  screenText.text = questionJson[i].start;
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
                            List<string> stringsToAdd = SplitStrings(text[j]);
                            for (int k = 0; k < stringsToAdd.Count; k++)
                            {
                                questionLines.Add(stringsToAdd[k]);
                            }
                        }
                    }
                    contraDictIndex++;
                }

                //number of contradictory words == the place in the contradiction chain
                if((questionJson[i].contra.Count==placeInChain&&placeInChain!=0))
                {
                    if (currentLevel == 0)
                    {
                        Debug.Log("THAT'S NEXT LEVEL!");
                        currentLevel++;
                        listenJson = listenJsonParser.parseLevelJson("levelTwo");
                        questionJson = questionJsonParser.parseLevelJson("levelTwo");

                    }
                    totalContradictions++;
                }

                if (questionLines.Count == 0)
                {
                    if (questionJson[i].flavor.TryGetValue(word, out text))
                    {
                        placeInChain = 0;
                        for (int j = 0; j < text.Count; j++)
                        {
                            List<string> stringsToAdd = SplitStrings(text[j]);
                            for (int k = 0; k < stringsToAdd.Count; k++)
                            {
                                questionLines.Add(stringsToAdd[k]);
                            }
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
        StartCoroutine("TypeText", questionLines[currentLine]);

        //screenText.text = questionLines[currentLine];
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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (prevScene == 1)
            {
                playerMovement.gameObject.transform.parent.position = tentExitPositions[0].position;
            }
            if (prevScene == 2)
            {
                playerMovement.gameObject.transform.parent.position = tentExitPositions[1].position;
            }
            if (prevScene == 3)
            {
                playerMovement.gameObject.transform.parent.position = tentExitPositions[2].position;
            }
            if (prevScene == 4)
            {
                playerMovement.gameObject.transform.parent.position = tentExitPositions[3].position;
            }
            if (prevScene == 5)
            {
                playerMovement.gameObject.transform.parent.position = tentExitPositions[4].position;
            }
        }
    }

    public IEnumerator TypeText(string input)
    {
        if (scrollingTextDone)
        {

            screenText.text = "";
            scrollingTextDone = false;
            bool rich=false;
            string letter = "";
            string total="";
            int count=0;
            for (int i=0;i<input.Length;i++)
            {
                letter = input[i] + "";
                if(letter=="<")
                {
                    rich = true;
                    total+=letter;
                }
                else if(rich&&letter != ">")
                {
                    total += letter;
                }
                else if(rich && letter == ">")
                {
                    total += letter;
                    count++;
                    if (count == 2)
                    {
                        screenText.text = total;
                        rich = false;
                        total = "";
                    }
                }
                else
                {
                    screenText.text += letter;
                }

                if(rich)
                {
                    yield return new WaitForEndOfFrame();
                }
                else
                    yield return new WaitForSeconds(.001f);

            }

            scrollingTextDone = true;
        }
        else
        {
            screenText.text = input;
            scrollingTextDone = true;
            StopAllCoroutines();
            yield return new WaitForSeconds(.001f);
        }
    }

    public List<string> SplitStrings(string toSplit)
    {

        List<string> allStrings=new List<string>();
        string tempString;

        if(CalculateLengthOfMessage(toSplit)[0]<=1800)
        {
            allStrings.Add(toSplit);
            return allStrings;
        }
        int start = 0;
        int end = CalculateLengthOfMessage(toSplit)[1];
        while(toSplit[end]+""!=" ")
        {
            end--;
        }
        tempString = toSplit.Substring(start, end)+ "–";
        allStrings.Add(tempString);
        tempString = "–"+ toSplit.Substring(end, toSplit.Length-end);
        allStrings.Add(tempString);
        return allStrings;

    }


    int[] CalculateLengthOfMessage(string message)
    {
        int[] lengthThenSplitSpot = new int[2];
        int totalLength = 0;
        Font myFont = screenText.font;  //chatText is my Text component
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = message.ToCharArray();
        int count = 0;
        foreach (char c in arr)
        {
            myFont.RequestCharactersInTexture(message, 18);

            myFont.GetCharacterInfo(c, out characterInfo, 18);
            totalLength += characterInfo.advance;
            if (totalLength > 1800 && lengthThenSplitSpot[1] == 0)
            {
                lengthThenSplitSpot[1] = count;
            }
            count++;
        }
        lengthThenSplitSpot[0] = totalLength;
        return lengthThenSplitSpot;
    }
    public void LoadFinalScene()
    {
        SceneManager.LoadScene(6);
    }
}

