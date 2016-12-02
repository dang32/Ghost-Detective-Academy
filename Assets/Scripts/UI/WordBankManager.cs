using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WordBankManager : MonoBehaviour {

	public GameObject noteButton;
	public GameObject wordBank;
	public GameObject[] notes;
	public Text note;
	public Canvas noteCanvas;
	public TextBoxManager textManager;

	private string jsonFile;
	private JsonData jsonData;

	public List<string> words = new List<string>();

	public bool wordBankActive;
	public bool noteButtonActive;
    public GameObject hideNotepadButton;
	// Use this for initialization
	void Start () {
		DisableWordBank();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onNoteClick() {
//		jsonFile = File.ReadAllText(Application.dataPath + "/Text/ListenJSON.json");
//		jsonData = JsonMapper.ToObject(jsonFile);
//		for (int i = 0; i < jsonData["levelOne"][0]["wordBank"].Count; i++)
//		{
//			words.Add((jsonData["levelOne"][0]["wordBank"][i]).ToString());
//		}
		EnableWordBank();
	}

//	public string onWordClick(string name){
//		Debug.Log("here " + name);
//		return name;
//	}

	public void EnableWordBank(){
		wordBank.SetActive(true);
        wordBankActive = true;
		//noteButton.SetActive(false);
		noteButtonActive = false;
	}

	public void DisableWordBank(){
		wordBank.SetActive(false);
        //wordBankActive = false;
		//noteButton.SetActive(true);
        noteButtonActive = true;
    }

    public void AddWordsToBank()
    {
        wordBankActive = true;
    }

    public void AddWord(string word){
		words.Add(word);
	}
}
