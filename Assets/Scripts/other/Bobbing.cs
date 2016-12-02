using UnityEngine;
using System.Collections;

public class Bobbing : MonoBehaviour {
    Vector3 startPos;
    public float bobbingHeight; //how much the ghost goes up and down
    public float bobbingSpeed;
	// Use this for initialization
	void Start () {
        startPos = transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position=new Vector3(transform.position.x, startPos.y+ (Mathf.Sin(Time.time/ bobbingSpeed) * bobbingHeight), transform.position.z);
	}
}
