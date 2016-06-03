using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChange : MonoBehaviour {

    public string sceneToLoad = "03-MainLevels";
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
        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }

        if (sceneTransition.alpha >= 1 && startTransition)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Collider"))
        {
            startTransition = true;

            PlayerPrefs.SetInt("hullIntegrity", other.gameObject.GetComponentInParent<PlayerController>().getCurrHullIntegrity());
            PlayerPrefs.SetInt("bombCharge", other.gameObject.GetComponentInParent<PlayerController>().getBombCharge());
            PlayerPrefs.SetFloat("shieldCharge", other.gameObject.GetComponentInParent<PlayerController>().getShieldCharge());
            PlayerPrefs.Save();
        }
    }

}
