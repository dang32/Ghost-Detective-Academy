﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateText : MonoBehaviour {
    public GameObject dialogueBoxes;
    public GameObject listen;
    public GameObject question;
    public GameObject text;
    public ViewSwitch cameraSwitcher;
    public PlayerMovement player;
    public PlayerHistory playerHistory;
	public TextBoxManager textManager;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerMovement>();
		playerHistory = FindObjectOfType<PlayerHistory>();
		cameraSwitcher = FindObjectOfType<ViewSwitch>();
	}

	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(player.transform.position, transform.position) < 3&&Camera.main.fieldOfView>58){

        }
	}
    void OnTriggerEnter(Collider col)
    {

            Debug.Log("here");
            textManager.EnableTextBox();
            textManager.npcName = transform.name;
            cameraSwitcher.isTalking = true;
            dialogueBoxes.SetActive(true);
            listen.SetActive(true);
            if (playerHistory.wordList.Count == 0)
            {
                question.SetActive(false);
            }
            else
            {
                question.SetActive(true);
            }
            text.SetActive(true);
            for (int i = 0; i < textManager.listenJson.Count; i++)
            {
                if (textManager.listenJson[i].name == textManager.npcName)
                {
                    text.GetComponent<Text>().text = textManager.listenJson[i].start;
                    textManager.defaultText = textManager.listenJson[i].start;
                }
            }


        
    }
}
