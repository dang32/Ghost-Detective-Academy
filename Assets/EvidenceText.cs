using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class EvidenceText : MonoBehaviour {
    public float appearDuration;
    public Image evidenceImage;
    Text evidenceAppearText;
    public string[] evidenceDescriptions;
    public Sprite[] imagesOfEvidence;
	// Use this for initialization
	void Start () {
        evidenceAppearText = transform.GetChild(3).gameObject.GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Appear(int index)
    {
        Debug.Log("evidence appear");
        evidenceAppearText.text = evidenceDescriptions[index];
        evidenceImage.sprite = imagesOfEvidence[index];
       for (int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void Disappear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
