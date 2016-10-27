using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public bool frozen;
    Rigidbody rigi;

    // Use this for initialization
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("Vertical") + " " + Input.GetAxis("Horizontal"));
        rigi.velocity = Vector3.zero;
        if (!frozen)
        {
            transform.Translate(-Input.GetAxis("Vertical") * speed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * speed * Time.deltaTime, Space.World);

            if (Input.anyKey && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                transform.forward = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            }
        }
    }
}
