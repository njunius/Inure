using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsToMainMenu : MonoBehaviour {

    private CanvasGroup sceneTransition;
    private AudioSource backgroundMusic;


    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;
    
    // Use this for initialization
    void Start () {
        sceneTransition = GameObject.FindGameObjectWithTag("Screen Transition").GetComponent<CanvasGroup>();
        backgroundMusic = GetComponent<AudioSource>();

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
            SceneNumHolder.cachedSceneNum = 2; // 2 is the main menu index
            SceneManager.LoadScene("LoadingScreen");
        }

        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;

            backgroundMusic.volume -= transitionRate * Time.unscaledDeltaTime; //fade out music with the screen fade
        }
    }

    public void endCredits()
    {
        startTransition = true;
    }
}
