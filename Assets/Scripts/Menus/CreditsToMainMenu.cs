using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsToMainMenu : MonoBehaviour {

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
        AudioListener.pause = false;

    }

    // Update is called once per frame
    void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            startTransition = true;
            AudioListener.pause = true;

        }

        if (transitionCounter >= 1)
        {
            SceneManager.LoadScene("00-MainMenu");
        }

        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }
    }

    public void endCredits()
    {
        AudioListener.pause = true;
        startTransition = true;
    }
}
