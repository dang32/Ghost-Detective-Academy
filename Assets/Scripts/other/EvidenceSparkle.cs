using UnityEngine;
using System.Collections;

public class EvidenceSparkle : MonoBehaviour {
    public int index;
    EvidenceManager evidenceMana;
    // Use this for initialization
    void Start () {
        evidenceMana = GameObject.Find("persistent obj").GetComponent<EvidenceManager>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(transform.GetChild(0).gameObject.activeSelf)
        {
            evidenceMana.FoundEvidence(index);
            DisableSparkle();
        }
      
    }

    public void EnableSparkle()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DisableSparkle()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
