using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateText : MonoBehaviour {
    public GameObject dialogueBoxes;
    public GameObject listen;
    public GameObject question;
    public GameObject text;
    public ViewSwitch cameraSwitcher;
    public PlayerMovement player;
	public TextBoxManager textManager;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerMovement>();
		textManager = FindObjectOfType<TextBoxManager>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Vector3.Distance(player.transform.position, transform.position) < 3&&Camera.main.fieldOfView>58){
			if(Input.GetKey(KeyCode.Space)){
                //Debug.Log("here");
                textManager.EnableTextBox();
                cameraSwitcher.isTalking = true;
                dialogueBoxes.SetActive(true);
                listen.SetActive(true);
                question.SetActive(true);
                text.SetActive(true);
                text.GetComponent<Text>().text = "Hi, how are you?";
            }
        }
	}
}
