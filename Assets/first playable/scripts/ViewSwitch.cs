using UnityEngine;
using System.Collections;

public class ViewSwitch : MonoBehaviour {
    public PlayerMovement charMovement;
    public Eyes clownEyes;
    public Transform clown;
    public Transform player; //main char model
    Vector3 offsetFromPlayer; //offset of camera for overhead view
    float camHeight; //height of cam off ground

    Vector3 playerStartPos; //main char pos before talking to npc
    Quaternion playerStartRot;//main char rot before talking to npc
    public Transform playerTalkingTrans;//place where the main char should be when talking to npc

    public float transitionTime;//duration it takes for camera to move
    public float elapsedTime;//keeps track of how long camera is moving
    public bool zoomed;//true if camera is zoomed in, false otherwise

    public Transform zoomedTrans; //where the camera goes when talking to npc
    Quaternion overheadRot; //the camera rotation of the overhead perspective

    public float fovStart; //start fov, for overhead
    public float fovEnd;//end fov for talking view
    Camera cam;
    public bool isTalking;
    public Color fogColor;
    public TextBoxManager textBoxMana;
    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        overheadRot = transform.rotation;
        offsetFromPlayer = transform.position - player.position;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = .015f;
        RenderSettings.fogColor = fogColor;
        camHeight = transform.position.y;

    }

    // Update is called once per frame
    void Update () {

       
            charMovement.frozen = zoomed;
        if(clownEyes!= null)
        clownEyes.zoomed = zoomed;
        

        //switch between zoomed and overhead by pressing space
        if (Input.GetKeyDown(KeyCode.Space)&&elapsedTime==0&&Vector3.Distance(player.position,clown.position)<3) 
        {
        Debug.Log("space pressed");
            if (!zoomed)
            {
                playerStartPos = player.position;
                playerStartRot = player.rotation;
                StartCoroutine("Zoom");
                zoomed = true;

            }
            //switch to overhead
            else if(!isTalking)
            {
            Debug.Log("overhead triggered");
                StartCoroutine("Overhead");
                zoomed = false;
            }
        }

        //stop coroutines once their set duration ends
        if (elapsedTime > transitionTime) 
        {
            elapsedTime = 0;
            StopAllCoroutines();
        }

        if(cam.fieldOfView> fovStart-1)
        {
            //makes camera follow player if it could move
            transform.position = Vector3.Lerp(
                transform.position,
                Vector3.Scale(player.position+offsetFromPlayer,new Vector3(1,0,1))+new Vector3(0,camHeight,0),
                Time.deltaTime*12);
        }

    }

    private IEnumerator Zoom()
    {
        //set position, rotation, fieldOfView, and fog   when switching between zoomed and overhead
        while (elapsedTime < transitionTime)
        {
            Vector3 startPos = Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0);

            player.position= Vector3.Lerp(playerStartPos, playerTalkingTrans.position, elapsedTime / transitionTime);
            player.rotation = Quaternion.Lerp(playerStartRot, playerTalkingTrans.rotation, elapsedTime / transitionTime);

            transform.position = Vector3.Lerp(startPos, zoomedTrans.position, elapsedTime/transitionTime);
            transform.rotation = Quaternion.Lerp(overheadRot, zoomedTrans.rotation, elapsedTime / transitionTime);
            cam.fieldOfView = Mathf.Lerp(fovStart, fovEnd, elapsedTime / transitionTime);
            RenderSettings.fogDensity = Mathf.Lerp(.015f, 0, elapsedTime / transitionTime);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator Overhead()
    {
        while (elapsedTime < transitionTime)
        {
            Vector3 startPos = Vector3.Scale(player.position + offsetFromPlayer, new Vector3(1, 0, 1)) + new Vector3(0, camHeight, 0);
            player.position = Vector3.Lerp( playerTalkingTrans.position, playerStartPos, elapsedTime / transitionTime);
            player.rotation = Quaternion.Lerp(playerTalkingTrans.rotation, playerStartRot, elapsedTime / transitionTime);
            transform.position = Vector3.Lerp(zoomedTrans.position, startPos, elapsedTime / transitionTime);
            transform.rotation = Quaternion.Lerp(zoomedTrans.rotation, overheadRot, elapsedTime / transitionTime);
            cam.fieldOfView = Mathf.Lerp(fovEnd, fovStart, elapsedTime / transitionTime); //60 is the default fov
            RenderSettings.fogDensity = Mathf.Lerp(0, .015f, elapsedTime / transitionTime); //.1 is the level of fog

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void SwitchToOverhead()
    {
		StartCoroutine("Overhead");
        zoomed = false;
    }
}
