using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Instructions : MonoBehaviour {
    public string[] instructionArray;
    int index;
    public Text instructionText;
	// Use this for initialization
	void Start () {
        if (index < instructionArray.Length)

            instructionText.text = instructionArray[index];
    }

    // Update is called once per frame
    void Update () {
	
	}
    public void nextInstruction()
    {
        index++;
        if (index < instructionArray.Length)
            instructionText.text = instructionArray[index];
        else
            gameObject.SetActive(false);
    }
}
