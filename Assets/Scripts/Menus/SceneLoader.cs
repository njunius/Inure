using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    private bool loadScene;

    public int sceneNum;

    public CanvasGroup sceneTransition;

    public Text progressText;

    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;

    private AsyncOperation asyncProgress;

    // Use this for initialization
    void Start () {
        sceneNum = SceneNumHolder.cachedSceneNum;

        sceneTransition = GameObject.FindGameObjectWithTag("Screen Transition").GetComponent<CanvasGroup>();

        transitionRate = 3;
        transitionCounter = 0;

        progressText.text = "[Loading...0%]";
    }

    // Update is called once per frame
    void Update () {
        if (!loadScene)
        {
            loadScene = true;
            StartCoroutine(LoadNumScene());
        }
    }

    IEnumerator LoadNumScene()
    {
        while(sceneTransition.alpha > 0)
        {
            yield return null;
        }

        asyncProgress = SceneManager.LoadSceneAsync(sceneNum);
        asyncProgress.allowSceneActivation = false;

        while(asyncProgress.progress < 0.9f)
        {
            progressText.text = "[Loading..." + (asyncProgress.progress / 0.9f * 100f).ToString("00.00") + "%]";
            yield return null;
        }

        yield return StartCoroutine(FadeOut());

        //yield return new WaitForEndOfFrame();

        asyncProgress.allowSceneActivation = true;

    }

    IEnumerator FadeOut()
    {
        while (transitionCounter < 1)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;
            sceneTransition.alpha = transitionCounter;

            yield return new WaitForEndOfFrame(); //makes sure the screen has completely faded before finalizing the load

        }

    }

}
