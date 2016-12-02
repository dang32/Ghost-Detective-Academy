using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class TextImporter : MonoBehaviour {

	private string jsonFile;
	private JsonData jsonData;
	public List<string> textLines = new List<string>();

	// Use this for initialization
	void Start () {
		jsonFile = File.ReadAllText(Application.dataPath + "/Text/ListenJSON.json");
		jsonData = JsonMapper.ToObject(jsonFile);
		for (int i = 0; i < jsonData["levelOne"][0]["dialogue"].Count; i++)
		{
			Debug.Log(i);
			textLines.Add((jsonData["levelOne"][0]["dialogue"][i]).ToString());
		}
		Debug.Log(textLines);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
