using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnitySampleAssets.ImageEffects;
using System;

namespace UnityStandardAssets.ImageEffects
{
    public class Instructions : MonoBehaviour
    {
        bool moreEvidence;
        public int extraInstructionShown;
        public List<string> instructionArray;
        public int index;
        public Text instructionText;
        public bool introDone;
        public List<GameObject> alts;
        public PlayerMovement player;
        public ScreenOverlay background;
        public GameObject noteButton;
        public TextBoxManager textBoxes;
        public EvidenceManager evidence;
        public GameObject evidenceObj;
        public FinalManager final;
        public Texture2D black;
        public GameObject exitButton;
        // Use this for initialization


        void Start()
        {
            instructionArray = new List<string>();
            
            
            instructionArray.Add("Detective: The name’s Boolean. Detective Boolean. I run the Ghost Detective Agency here in the ghost realm, a halfway point between the land of the living and eternity.");
            instructionArray.Add("A sort of place to figure out your junk and move on so you can go to the next realm. I solve all of the cold cases you can’t figure out on Earth. In the ghost realm, the most common reason to be stuck here is murder, so I have a long waiting list.");
            instructionArray.Add("We’ll start this story off with the hottest cold case I’d ever seen, a case I’d written off as a lost cause until, generations later, it suddenly popped on my desk again.");
            instructionArray.Add("");
            instructionArray.Add("");
            instructionArray.Add("");
            instructionArray.Add("Detective: Marionetta the Ventriloquist. The last living witness to the crime has finally appeared in the ghost realm. I pray she’ll be enough to solve this case. I use Marionetta as a focus to get to the crime scene as it was so many years ago, the witnesses waiting in their tents, brought in, once again, by Mr. G. Reaper.");
            instructionArray.Add("A clown, two trapeze artists, a strongman, a sword swallower, and a ventriloquist. Any of these oddities could have done the good Ringmaster in. I choose my first witness.");
            instructionArray.Add("Instructions: \n1) Find suspects to invesigate\n2) Listen to their tesitmonies and identify key words\n3) Question other suspects using chains of these key words to find contradictions\n4) Gather six pieces of evidence (look for sparkles) for your final accusation \n(WSAD to move, esc to quit)");
            if (!introDone)
            {
                background.intensity = 1;
                if(player!=null)
                player.frozen = true;
                noteButton.SetActive(false);
                noteButton.GetComponent<Button>().enabled = false;
                noteButton.GetComponent<Image>().enabled = false;
                noteButton.transform.GetChild(0).gameObject.SetActive(false);
                if (index < instructionArray.Count)
                    instructionText.text = instructionArray[index];
            }


            
        }

        // Update is called once per frame
        void Update()
        {
            if (player==null&&textBoxes.playerMovement != null)
                player = textBoxes.playerMovement;

            if (final.fadeToBlack)
            {
                final.fadeToBlack = false;
                StartCoroutine("FadetoBlack");
            }
            if (index==0)
            {
                player.frozen = true;
            }
            if(index>8&&background==null&&SceneManager.GetActiveScene().buildIndex==0)
            {
                background = Camera.main.GetComponent<ScreenOverlay>();
                background.enabled = false;
            }

            if(extraInstructionShown==0&&SceneManager.GetActiveScene().buildIndex>0)
            {
                extraInstructionShown++;
                transform.GetChild(6).gameObject.SetActive(true);
            }

            if (SceneManager.GetActiveScene().buildIndex!=6&& SceneManager.GetActiveScene().buildIndex != 0&& extraInstructionShown == 1 && textBoxes.characterWantsToSayLevel[SceneManager.GetActiveScene().buildIndex-1] == 1 && textBoxes.isActive == false)
            {
                extraInstructionShown++;
                transform.GetChild(7).gameObject.SetActive(true);
            }

            if (extraInstructionShown == 2 && textBoxes.currentLevel==1 && textBoxes.endAtLine == -1)
            {
                textBoxes.DisableTextBox();
                extraInstructionShown++;
                transform.GetChild(8).gameObject.SetActive(true);
            }

            if (textBoxes.totalContradictions == 2  &&evidence.totalEvidenceFound==6&&SceneManager.GetActiveScene().buildIndex!=6)
            {
                if(textBoxes.isActive&& textBoxes.endAtLine == -1)
                textBoxes.DisableTextBox();
                else if(!textBoxes.isActive)
                transform.GetChild(9).gameObject.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 6)
                transform.GetChild(9).gameObject.SetActive(false);

            if (textBoxes.totalContradictions == 2 && textBoxes.endAtLine == -1 &&evidence.totalEvidenceFound!=6&& !moreEvidence && SceneManager.GetActiveScene().buildIndex != 6)
            {
                if(textBoxes.isActive)
                textBoxes.DisableTextBox();
                moreEvidence = true;
                transform.GetChild(11).gameObject.SetActive(true);
            }
            else if(SceneManager.GetActiveScene().buildIndex==6)
            {
                transform.GetChild(11).gameObject.SetActive(false);

            }
            //  if (evidence.totalEvidenceFound == 6 && textBoxes.isActive == false&&textBoxes.)

        }
        public void nextInstruction()
        {
            index++;
            if (index < instructionArray.Count)
                instructionText.text = instructionArray[index];
            else {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                transform.GetChild(5).gameObject.SetActive(false);
                introDone = true;
                player.frozen = false;
                noteButton.SetActive(true);
                noteButton.GetComponent<Button>().enabled = true;
                noteButton.GetComponent<Image>().enabled = true;
                noteButton.transform.GetChild(0).gameObject.SetActive(true);
                evidenceObj.SetActive(true);
                textBoxes.instructionsDone = true;
            }

            if (index == 3)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
            if (index == 4)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
            }
            if (index == 5)
            {
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(true);
            }
            if (index == 6)
            {
                transform.GetChild(3).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
            }
            if (index == 8)
            {
                StartCoroutine("FadeBackground");
            }
        }
        IEnumerator FadeBackground()
        {
            float elapsedTime=0;
            while (elapsedTime<=2)
            {
                elapsedTime += Time.deltaTime;
                background.intensity = Mathf.Lerp(background.intensity, 0, elapsedTime / 2);
                yield return new WaitForEndOfFrame();

            }
        }

        IEnumerator FadetoBlack()
        {
            background = Camera.main.gameObject.GetComponent<ScreenOverlay>();
            background.texture = black;
            float elapsedTime = 0;
            while (elapsedTime <= 6)
            {
                elapsedTime += Time.deltaTime;
                background.intensity = Mathf.Lerp(background.intensity,1, elapsedTime / 6);

                if(elapsedTime>2)
                {
                    exitButton.SetActive(true);
                }
                yield return new WaitForEndOfFrame();

            }
            
        }
        public void Leave()
        {
            Application.Quit();
        }
    }
}