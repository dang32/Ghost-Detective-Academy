using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mainCharacterMovement : MonoBehaviour {
    public float speed;
    public bool frozen;


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (!frozen)
        {
            transform.Translate(-Input.GetAxis("Vertical") * speed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * speed * Time.deltaTime, Space.World);

            if (Input.anyKey&&(Input.GetAxis("Horizontal")!=0&& Input.GetAxis("Vertical")!=0))
            {
                transform.forward = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            }
        }
    }
}
