using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadSceneOnTrigger : MonoBehaviour {
    [SerializeField]
    public int sceneToSwitchTo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.name== "main char")
        SceneManager.LoadScene(sceneToSwitchTo);
    }
}
