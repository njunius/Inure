using UnityEngine;
using System.Collections;

public class ScreenTransitionFadeController : MonoBehaviour {

    private CanvasGroup sceneTransition;

    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;

    // Use this for initialization
    void Start () {

        sceneTransition = GetComponent<CanvasGroup>();

        startTransition = true;
        transitionRate = 3;
        transitionCounter = 1;

        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (startTransition)
        {
            transitionCounter -= transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }
    }
}
