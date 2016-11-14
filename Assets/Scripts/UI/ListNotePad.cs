using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListNotePad : MonoBehaviour {

	public PlayerHistory playerHistory;
	public TextBoxManager textManager;
	public WordBankManager wordManager;
	public Text newWord;
	private GameObject notePadPanel;
	public GameObject buttonPrefab;
	private List<string> created = new List<string>();
    Navigation customNav;
    public List<string> notePad = new List<string>();

	// Use this for initialization
	void Start () {
        playerHistory = FindObjectOfType<PlayerHistory>();
        notePadPanel = GameObject.Find("NotePanel");
        customNav = new Navigation();
        customNav.mode= Navigation.Mode.None;
    }
	
	// Update is called once per frame
	void Update () {
		if (wordManager.wordBankActive == true) {
			notePad = playerHistory.wordList;
			for (int i = 0; i < notePad.Count; i++)
			{
				if (!created.Contains(notePad[i]))
				{
					GameObject newButton = (GameObject)Instantiate(buttonPrefab);
					newButton.GetComponentInChildren<Text>().text = notePad[i];
					newButton.transform.SetParent(notePadPanel.transform, false);
					//Vector3 moveDown = newButton.transform.position;
					newButton.GetComponentInChildren<Button>().onClick.AddListener(() => textManager.onWordClick(newButton.GetComponentInChildren<Text>().text));
					//moveDown.y -= 34f * (float)i;
					//newButton.transform.position = moveDown;
                    newButton.GetComponent<Button>().navigation= customNav;
					created.Add(notePad[i]);
					Debug.Log(notePad[i]);
				}
			}
            wordManager.wordBankActive = false;

        }
	}
}
