using UnityEngine;
using LitJson;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class parseListenJson
{
	public string name;
	public string start;
	public List<string> dialogue;
	public List<string> wordBank;
	public string color;
	public string speed;
}	

public class ListenJsonParser : MonoBehaviour {

	public List<parseListenJson> parseLevelJson(string level)
	{
		string listenFile = File.ReadAllText(Application.dataPath + "/Text/ListenJSON.json");
		JsonData listenData = JsonMapper.ToObject(listenFile);
		List<parseListenJson> listenList = new List<parseListenJson>();
		for (int i = 0; i < listenData[level].Count; i++)
		{
			parseListenJson parseJson = new parseListenJson();
			parseJson.name = listenData[level][i]["name"].ToString();
			parseJson.start = listenData[level][i]["start"].ToString();
			parseJson.color = listenData[level][i]["color"].ToString();
			parseJson.speed = listenData[level][i]["speed"].ToString();
			parseJson.dialogue = new List<string>();
			parseJson.wordBank = new List<string>();
			for (int j = 0; j < listenData[level][i]["dialogue"].Count; j++)
			{
				parseJson.dialogue.Add(listenData[level][i]["dialogue"][j].ToString());
			}
			for (int j = 0; j < listenData[level][i]["wordBank"].Count; j++)
			{
				parseJson.wordBank.Add(listenData[level][i]["wordBank"][j].ToString());
			}
			listenList.Add(parseJson);
		}
		return listenList;
	}
}
