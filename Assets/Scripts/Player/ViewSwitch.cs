using UnityEngine;
using System.Collections;

public class ViewSwitch : MonoBehaviour {
    public PlayerMovement charMovement;
    public Eyes clownEyes;
    public Transform clown;
    public Transform player; //main char model
    Vector3 offsetFromPlayer; //offset of camera for overhead view
    float camHeight; //height of cam off ground

    public Vector3 playerStartPos; //main char pos before talking to npc
    public Quaternion playerStartRot;//main char rot before talking to npc
    public Transform playerTalkingTrans;//place where the main char should be when talking to npc

    public float transitionTime;//duration it takes for camera to move
    public float elapsedTime;//keeps track of how long camera is moving
    public bool zoomed;//true if camera is zoomed in, false otherwise
    
    public Transform zoomedTrans; //where the camera goes when talking to npc
    Quaternion overheadRot; //the camera rotation of the overhead perspective
    public GameObject smoke;
    public float fovStart; //start fov, for overhead
    public float fovEnd;//end fov for talking view
    public Camera cam;
    public bool isTalking;
    public bool transitioning;
  //  public Color fogColor;
    public TextBoxManager textBoxMana;
    public Light faceLight;
    public bool outside;
    Rigidbody rigi;
    Vector3 direction;
    // Use this for initialization
    void Start () {
        rigi = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();

        charMovement = GameObject.Find("main char").GetComponent<PlayerMovement>();
        player = GameObject.Find("main char").transform;
        if (!outside)
        {
            clown = FindObjectOfType<Eyes>().transform.parent;
            clownEyes = FindObjectOfType<Eyes>();

            textBoxMana = GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>();
            faceLight = GameObject.Find("face light").GetComponent<Light>();

            zoomedTrans = GameObject.Find("zoom pos camera").transform;
            playerTalkingTrans = GameObject.Find("talking pos main char").transform;
        }
        overheadRot = transform.rotation;
        offsetFromPlayer = transform.position - player.position;
        RenderSettings.fogMode = FogMode.Exponential;
      //  RenderSettings.fogDensity = .015f;
      //  RenderSettings.fogColor = fogColor;
        camHeight = transform.position.y;

    }

    // Update is called once per frame
    void FixedUpdate () {

       
        if(clownEyes!= null)
        clownEyes.zoomed = zoomed;
        

        ////switch between zoomed and overhead by pressing space
        //if (Input.GetKeyDown(KeyCode.Space)&&elapsedTime==0&&Vector3.Distance(player.position,clown.position)<3) 
        //{
        //    if (!zoomed)
        //    {
        //        playerStartPos = player.position;
        //        playerStartRot = player.rotation;
        //      //  StartCoroutine("Zoom");
        //        zoomed = true;

        //    }
        //    //switch to overhead
        //    else if(!isTalking)
        //    {
        //    Debug.Log("overhead triggered");
        //       // StartCoroutine("Overhead");
        //        zoomed = false;
        //    }
        //}

        //stop coroutines once their set duration ends
        if (elapsedTime > transitionTime) 
        {
            elapsedTime = 0;
            StopAllCoroutines();
            transitioning = false;

        }
        rigi.velocity = Vector3.zero;
        if (cam.fieldOfView> fovStart-4)
        {
            
            rigi.position = Vector3.Lerp(
                            rigi.position,
                            Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0),
                            Time.deltaTime * 12);
                            
           
            //makes camera follow player if it could move
            /*
            transform.position = Vector3.Lerp(
                transform.position,
                Vector3.Scale(player.position+offsetFromPlayer,new Vector3(1,0,1))+new Vector3(0,camHeight,0),
                Time.deltaTime*12);
                */
            // rigi.AddForce((Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0))- (transform.position));
            //direction = ((Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0)) - transform.position).normalized;
            // rigi.MovePosition(rigi.position + direction * 3f * Time.deltaTime);
            /*
            rigi.MovePosition(Vector3.Lerp(
                  rigi.position,
                  Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0),
                  Time.deltaTime * 12));
                  */
        }

    }

    public IEnumerator Zoom()
    {
        smoke.SetActive(true);
        zoomed = true;
        charMovement.frozen = true;
  //      playerStartPos = player.position;
  //      playerStartRot = player.rotation;
        //set position, rotation, fieldOfView, and fog   when switching between zoomed and overhead
        while (elapsedTime < transitionTime)
        {

            Vector3 startPos = Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0);

            player.position= Vector3.Slerp(playerStartPos, playerTalkingTrans.position, elapsedTime / transitionTime);
            player.rotation = Quaternion.Slerp(playerStartRot, playerTalkingTrans.rotation, elapsedTime / transitionTime);

            transform.position = Vector3.Lerp(startPos, zoomedTrans.position, elapsedTime/transitionTime);
            transform.rotation = Quaternion.Lerp(overheadRot, zoomedTrans.rotation, elapsedTime / transitionTime);
            cam.fieldOfView = Mathf.Lerp(fovStart, fovEnd, elapsedTime / transitionTime);
            RenderSettings.fogDensity = Mathf.Lerp(.015f, .005f, elapsedTime / transitionTime);
            faceLight.intensity = Mathf.Lerp(faceLight.intensity, 1.7f, elapsedTime / transitionTime);

            elapsedTime += Time.deltaTime;

                transitioning = true;
            
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator Overhead()
    {
        smoke.SetActive(false);

       charMovement.frozen = true;
        Debug.Log("Overhead camera");
 //       playerStartPos = player.position;
  //      playerStartRot = player.rotation;
        while (elapsedTime < transitionTime)
        {
            zoomed = false;
            Vector3 startPos = Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0);
            //player.position = Vector3.Slerp( playerTalkingTrans.position, playerStartPos, elapsedTime / transitionTime);
            //player.rotation = Quaternion.Slerp(playerTalkingTrans.rotation, playerStartRot, elapsedTime / transitionTime);
            transform.position = Vector3.Lerp(zoomedTrans.position, startPos, elapsedTime / transitionTime);
            transform.rotation = Quaternion.Lerp(zoomedTrans.rotation, overheadRot, elapsedTime / transitionTime);
            cam.fieldOfView = Mathf.Lerp(fovEnd, fovStart, elapsedTime / transitionTime); //60 is the default fov
            RenderSettings.fogDensity = Mathf.Lerp(.005f, .015f, elapsedTime / transitionTime); //.1 is the level of fog
            faceLight.intensity = Mathf.Lerp(faceLight.intensity, 0, elapsedTime / transitionTime);

            elapsedTime += Time.deltaTime;
            if (!(elapsedTime < transitionTime))
            {
                charMovement.frozen = false;
            }
            else
            {
                transitioning = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    public void SwitchToOverhead()
    {
		StartCoroutine("Overhead");
        zoomed = false;
    }
}
