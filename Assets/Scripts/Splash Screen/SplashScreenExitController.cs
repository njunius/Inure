using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenExitController : MonoBehaviour {

    public string sceneToLoad;
    private CanvasGroup sceneTransition;

    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;

    // Use this for initialization
    void Start () {
        sceneTransition = GameObject.FindGameObjectWithTag("Screen Transition").GetComponent<CanvasGroup>();

        startTransition = false;
        transitionRate = 3;
        transitionCounter = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (transitionCounter >= 1)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }

        if (Input.anyKey)
        {
            startTransition = true;
        }
    }
}
