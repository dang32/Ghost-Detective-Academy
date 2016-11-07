using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {
    public bool zoomed;
    public Texture blankFace;
    public Texture fullFace;
    public Texture pupils;

    public Texture halfClosed;
    public Texture fullClosed;
    public Texture blank;

    float waitTime;
    float waitTime2;

    public float blinkAboutEverySeconds;
    public float blinkSpeed;
    float timeUntilBlink;
    float blinkStartTime;

    float t; //time gone by
    float t2; //time gone by

    public Material mat;
    Vector2 target;
    Vector2 pos;
    Vector2 offset;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    public float minXmicro;
    public float maxXmicro;

    public float minYmicro;
    public float maxYmicro;
    public int eyeMatIndex;
    // Use this for initialization
    void Start () {
        //Material[] allMats;
        //allMats = 
        
        mat = GetComponent<Renderer>().materials[eyeMatIndex];
	}
	
	// Update is called once per frame
	void Update () {
       

        timeUntilBlink += Time.deltaTime;

        if (timeUntilBlink> blinkAboutEverySeconds)
        {
            
            timeUntilBlink = 0;
            blinkStartTime = Time.time;
            
        }

        if(Time.time - blinkStartTime < blinkSpeed&& blinkStartTime!=0)
        {
            mat.SetTexture("_MainTex", blankFace);
            mat.SetTexture("_MainTexB", blank);
            Blink(blinkStartTime);
        }
        if (zoomed)
        {
            pos = Vector2.Lerp(pos, target + offset, 10 * Time.deltaTime);


            mat.SetTextureOffset("_MainTexB", pos);
            if (waitTime < Time.time - t)
            {
                t = Time.time;
                waitTime = Random.Range(.1f, 2f);
                if (Random.Range(0f, 1f) < .75f && offset == Vector2.zero)
                {
                    target = new Vector2(0f, 0f);
                }
                else {
                    target = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    offset = Vector2.zero;
                }

            }
            if (waitTime2 < Time.time - t2)
            {
                t2 = Time.time;
                waitTime2 = Random.Range(.2f, .3f);
                if (target == new Vector2(0f, 0f))
                {
                    //offset = new Vector2(Random.Range(-.001f, .001f), Random.Range(-.001f, .001f));
                    offset = new Vector2(Random.Range(minXmicro, maxXmicro), Random.Range(minYmicro, maxYmicro));

                }
            }
        }
        GetComponent<Renderer>().materials[eyeMatIndex] = mat;
        
    }


      void Blink(float blinkStartTime)
    {




        float elapsedTime = Time.time - blinkStartTime;
        if (elapsedTime < .25f * blinkSpeed)
        {

            mat.SetTexture("_MainTexC", halfClosed);
        }
        if (elapsedTime > .25f * blinkSpeed && elapsedTime < .5f * blinkSpeed)
        {
            mat.SetTexture("_MainTexC", fullClosed);
        }
        if (elapsedTime > .5f * blinkSpeed && elapsedTime < .75f * blinkSpeed)
        {
            mat.SetTexture("_MainTexC", halfClosed);
        }
        if (elapsedTime > .75f * blinkSpeed)
        {
            mat.SetTexture("_MainTexC", blank);
            mat.SetTexture("_MainTex", fullFace);
            mat.SetTexture("_MainTexB", pupils);
        }
    }

    /*
    private IEnumerator blink()
    {
        float elapsedTime = 0;

        while (elapsedTime < .1f) //.1 is duration of blink
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime<.25f*.1f)
            {

                mat.SetTexture("_MainTexC", halfClosed);
            }
            if (elapsedTime > .25f&&elapsedTime < .5f * .1f)
            {
                mat.SetTexture("_MainTexC", fullClosed);
            }
            if (elapsedTime > .5f && elapsedTime < .75f * .1f)
            {
                Debug.Log("asdf2");

                mat.SetTexture("_MainTexC", halfClosed);
            }
            if (elapsedTime > .75f*.1)
            {
                Debug.Log("asdf");

                mat.SetTexture("_MainTexC", blank);
            }
        }
        GetComponent<Renderer>().materials[5] = mat;

        yield return StartCoroutine(openLids(.05f));
    }


    private IEnumerator closeLids(float time)
    {


        float elapsedTime = 0;
        float closePos=-.5f;
        while (elapsedTime < time)
        {
            closePos = Mathf.Lerp(closePos, -0.005f, (elapsedTime / time));
            mat.SetTextureOffset("_MainTexC", new Vector2(.005f,closePos));
            elapsedTime += Time.deltaTime;
            yield return  null;
        }
        yield return StartCoroutine(openLids(.05f)); 
    }
    private IEnumerator openLids(float time)
    {


        float elapsedTime = 0;
        float closePos = -0.005f;
        while (elapsedTime < time)
        {
            Debug.Log("asdf");
            closePos = Mathf.Lerp(closePos, -.5f, (elapsedTime / time));
            mat.SetTextureOffset("_MainTexC", new Vector2(.005f, closePos));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForEndOfFrame();

   

    }
     */
}
