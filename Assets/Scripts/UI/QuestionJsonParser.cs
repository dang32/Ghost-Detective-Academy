using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using LitJson;
using System.IO;

public class parseQuestionJson
{
	public string name;
	public string start;
	public Dictionary<string, List<string>> contra;
	public Dictionary<string, List<string>> flavor;
	public string defaultText;
}

public class QuestionJsonParser : MonoBehaviour {
	public List<parseQuestionJson> parseLevelJson(string level)
	{
		string questionFile = File.ReadAllText(Application.dataPath + "/Text/QuestionJSON.json");
		JsonData questionData = JsonMapper.ToObject(questionFile);
		List<parseQuestionJson> questionJson = new List<parseQuestionJson>();
		for (int i=0; i < questionData[level].Count; i++)
		{
			parseQuestionJson parseJson = new parseQuestionJson();
			parseJson.name =  questionData[level][i]["name"].ToString();
			parseJson.start = questionData[level][i]["start"].ToString();
			parseJson.defaultText = questionData[level][i]["default"].ToString();
			parseJson.contra = new Dictionary<string, List<string>>();
			parseJson.flavor = new Dictionary<string, List<string>>();
			for (int j = 0; j < questionData[level][i]["contra"].Count; j++)
			{
				foreach (string word in questionData[level][i]["contra"][j].Keys)
				{
					List<string> contraText = new List<string>();
					for (int k = 0;  k < questionData[level][i]["contra"][j][word].Count; k++)
					{
						contraText.Add(questionData[level][i]["contra"][j][word][k].ToString());
					}
					parseJson.contra.Add(word, contraText);
				}
			}
			for (int j = 0; j < questionData[level][i]["flavor"].Count; j++)
			{
				foreach (string word in questionData[level][i]["flavor"][j].Keys)
				{
					List<string> flavorText = new List<string>();
					for (int k = 0;  k < questionData[level][i]["flavor"][j][word].Count; k++)
					{
						flavorText.Add(questionData[level][i]["flavor"][j][word][k].ToString());
					}
					parseJson.flavor.Add(word, flavorText);
				}
			}
			questionJson.Add(parseJson);
		}
		return questionJson;
	}
}

