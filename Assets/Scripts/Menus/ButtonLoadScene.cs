using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonLoadScene : MonoBehaviour, IPointerClickHandler {

    public string sceneToLoad;
    public int sceneToLoadIndex;

    private CanvasGroup sceneTransition;

    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;

	// Use this for initialization
	void Start () {

        sceneTransition = GameObject.FindGameObjectWithTag("Screen Transition").GetComponent<CanvasGroup>();

        sceneToLoad = "LoadingScreen";

        startTransition = false;
        transitionRate = 3;
        transitionCounter = 0;
	}

    void Update()
    {

        if (transitionCounter >= 1)
        {
            SceneNumHolder.cachedSceneNum = sceneToLoadIndex;
            SceneManager.LoadScene(sceneToLoad);
        }

        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }
    }
	
	public void OnPointerClick(PointerEventData eventData) {
        startTransition = true;
        Time.timeScale = 1;
        AudioListener.pause = true;
	}
}
