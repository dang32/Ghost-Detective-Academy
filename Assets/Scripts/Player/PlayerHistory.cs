using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHistory : MonoBehaviour {

    public List<string> wordList;
    public Dictionary<string, List<bool>> hasTalked;
    // Use this for initialization
    void Start () {
        wordList = new List<string>();
        hasTalked = new Dictionary<string, List<bool>>();
        hasTalked.Add("Strongman", new List<bool> { false, false });
        hasTalked.Add("Clown", new List<bool> { false, false });
        hasTalked.Add("Trapeze", new List<bool> { false, false });
        hasTalked.Add("Ventriloquist", new List<bool> { false, false });
        hasTalked.Add("Sword", new List<bool> { false, false });
    }

    // Update is called once per frame
    void Update () {
	
	}


    public int alreadyTalked(string name)
    {
        if (hasTalked.ContainsKey(name))
        {
            for (int i = 0; i < hasTalked[name].Count; i++)
            {
                if (hasTalked[name][i] == false)
                {
                    return i;
                }
            }
            return -1;
        }
        else {
            return -2;
        }
    }

    public void addWord(string word)
    {
        wordList.Add(word);
    }

    public void removeWord(string word)
    {
        wordList.Remove(word);
    }
}
