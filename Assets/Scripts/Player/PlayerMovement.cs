using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public bool frozen;
    Rigidbody rigi;
    Vector3 lastInput;

    // Use this for initialization
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
     //   Debug.Log(Input.GetAxis("Vertical") + " " + Input.GetAxis("Horizontal"));
        rigi.velocity = Vector3.zero;
        if (!frozen)
        {
            if(lastInput != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lastInput, Vector3.up), 4 * Time.deltaTime);

            //      Debug.Log("not frozon");
            transform.Translate(-Input.GetAxis("Vertical") * speed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * speed * Time.deltaTime, Space.World);

            if (Input.anyKey && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                lastInput = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
                
            }
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(0);
        }
    }
}
