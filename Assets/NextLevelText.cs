using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextLevelText : MonoBehaviour {
    public float appearDuration;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Appear(int evidenceIndex)
    {

        float elapsedTime = 0;

        while (elapsedTime < appearDuration)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);


    }
}
