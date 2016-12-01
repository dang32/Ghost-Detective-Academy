using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class EvidenceText : MonoBehaviour {
    public float appearDuration;
    Text evidenceAppearText;
	// Use this for initialization
	void Start () {
        evidenceAppearText = GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Appear(int evidenceIndex)
    {
       
        float elapsedTime = 0;

        while (elapsedTime < appearDuration)
        {
            evidenceAppearText.enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        evidenceAppearText.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);


    }
}
