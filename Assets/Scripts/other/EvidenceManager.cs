using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EvidenceManager : MonoBehaviour
{
    public int[] sparkles; //all sparkles in current scene. 0 is unset, 1 if active, -1 if disabled. Order is Cut rope,1,2,3,4,5
    public int section; //if section is 0, first 3 sparkles show, if 1 next 3 show 
    int currentScene;
    public GameObject[] evidenceImages;
    public int totalEvidenceFound;
    public static EvidenceManager instance = null;//Static instance of GameManager which allows it to be accessed by any other script.


    //every sparkles has it's own script with an index saying which piece of evidence it is
    //when someone walks into it, this script is given the index and it enables the piece of evidence ui matched with it (by indices)
    //The sparkles list keeps track of the state of a sparkle,   0: means they're unset (not active), 1:they've been set to be active, -1: you've walked into them



    void Awake()
    {
        //magic that allows it to go between scenes
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        currentScene = -1;
        sparkles = new int[6];
    }



    // Update is called once per frame
    void Update()
    {
        //triggers when scene changes
        if (currentScene != SceneManager.GetActiveScene().buildIndex)
        {
            //gets all evidence ui gameobjects
            for (int i = 0; i < GameObject.Find("UICanvas").transform.GetChild(4).childCount; i++)
            {
                evidenceImages[i] = GameObject.Find("UICanvas").transform.GetChild(4).GetChild(i).gameObject;
                if (sparkles[i] == -1)
                {
                    evidenceImages[i].SetActive(true);
                }
            }
            ReLoadSparkles();
        }
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }





    //determines if sparkles in current scene should be showing
    void ReLoadSparkles()
    {
        //gets each sparkles in current scene
        foreach (GameObject sparkle in GameObject.FindGameObjectsWithTag("sparkles"))
        {
            int evidenceIndex = sparkle.GetComponent<EvidenceSparkle>().index;

            //if they're unset
            if (sparkles[evidenceIndex] == 0)
            {
                if (evidenceIndex < 3 && section == 0 || (evidenceIndex > 2 && section == 1))
                {
                    sparkles[evidenceIndex] = 1;
                    sparkle.GetComponent<EvidenceSparkle>().EnableSparkle();

                }
                else
                {
                    sparkle.GetComponent<EvidenceSparkle>().DisableSparkle();
                }
            }
            else if (sparkles[evidenceIndex] != 0)
            {
                //if they've been set to be on
                if (sparkles[evidenceIndex] == 1)
                {
                    sparkles[evidenceIndex] = 1;
                    sparkle.GetComponent<EvidenceSparkle>().EnableSparkle();
                }
                //if they've been walked into
                else
                {
                    sparkles[evidenceIndex] = -1;
                    sparkle.GetComponent<EvidenceSparkle>().DisableSparkle();
                    evidenceImages[evidenceIndex].SetActive(true);
                }
            }
        }
    }






    //when somenone walks into evidence
    public void FoundEvidence(int evidenceIndex)
    {
        sparkles[evidenceIndex] = -1;
        totalEvidenceFound++;
        evidenceImages[evidenceIndex].SetActive(true);

        if (totalEvidenceFound == 3)
        {
            section++;
            ReLoadSparkles();

        }
    }
}
