using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonLoadScene : MonoBehaviour, IPointerClickHandler {

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

    void Update()
    {
        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }

        if(sceneTransition.alpha >= 1 && startTransition)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
	
	public void OnPointerClick(PointerEventData eventData) {
        startTransition = true;
        Time.timeScale = 1;
	}
}
