using UnityEngine;
using System.Collections;

public class campFire : MonoBehaviour {
    Light l;
    float randomGoal;
    float timer;
    public float resetEverySeconds;
	// Use this for initialization
	void Start () {
        l = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if(timer>resetEverySeconds)
        {
            randomGoal = Random.Range(.5f,7f);
            timer = 0;
        }
        timer += Time.deltaTime;
        l.intensity = Mathf.Lerp(l.intensity, randomGoal,1*Time.deltaTime);
	}
}
