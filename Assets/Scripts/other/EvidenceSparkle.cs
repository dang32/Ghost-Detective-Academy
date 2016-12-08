﻿using UnityEngine;
using System.Collections;

public class EvidenceSparkle : MonoBehaviour {
    public int index;
    public EvidenceManager evidenceMana;
    AudioSource shovel;
    // Use this for initialization
    void Start () {
        evidenceMana = GameObject.Find("persistent obj").GetComponent<EvidenceManager>();
        shovel = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(transform.GetChild(0).gameObject.activeSelf&&col.name=="main char")
        {
            evidenceMana.FoundEvidence(index);
            DisableSparkle();
        }
      
    }

    public void EnableSparkle()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();

    }

    public void DisableSparkle()
    {
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().playbackSpeed = 4f;
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().gravityModifier = -.018f;
        GetComponent<BoxCollider>().enabled = false;
        shovel.Play();
    }

    public void DisableSparkleWithoutCloud()
    {
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().playbackSpeed = 4f;
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().gravityModifier = -.018f;
        GetComponent<BoxCollider>().enabled = false;
    }
}
