using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonReloadScene : MonoBehaviour, IPointerClickHandler {

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
        if (transitionCounter >= 1)
        {
            SceneNumHolder.cachedSceneNum = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("LoadingScreen");
        }

        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        startTransition = true;
    }
}
