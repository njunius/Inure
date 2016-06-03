using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneTimerController : MonoBehaviour {

    private CanvasGroup sceneTransition;

    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;

    private float goToCutsceneTimer;

    private Vector3 storedMousePosition;

    // Use this for initialization
    void Start () {

        sceneTransition = GameObject.FindGameObjectWithTag("Screen Transition").GetComponent<CanvasGroup>();

        storedMousePosition = Input.mousePosition;

        startTransition = false;
        transitionRate = 3;
        transitionCounter = 0;

        goToCutsceneTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if(goToCutsceneTimer > 120)
        {
            startTransition = true;
        }

        if (transitionCounter >= 1)
        {
            SceneManager.LoadScene("00-OpeningCutscene");
        }

        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }

        goToCutsceneTimer += Time.deltaTime;

        if(Input.anyKeyDown || Input.mousePosition != storedMousePosition)
        {
            goToCutsceneTimer = 0;
        }

        storedMousePosition = Input.mousePosition;
	}
}
