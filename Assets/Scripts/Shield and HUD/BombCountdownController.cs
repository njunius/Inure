using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BombCountdownController : MonoBehaviour {

    public GameObject bomb;
    private BombDetach detachScript;
    private Canvas timerTextCanavas;
    private Text timerText;

    private HUDColorController hudColorController;
    private string hudElementName;

    // Use this for initialization
    void Start()
    {
        hudElementName = "bomb";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();
        detachScript = bomb.GetComponent<BombDetach>();
        timerTextCanavas = GetComponent<Canvas>();
        timerText = GetComponentInChildren<Text>();

        timerText.color = hudColorController.getColorByString(hudElementName);

        timerTextCanavas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (detachScript.isDetached())
        {
            timerTextCanavas.enabled = true;
            if (detachScript.get_countdown() < 10)
            {
                timerText.text = "T-" + detachScript.get_countdown().ToString("0.00") + "s";
            }
            else
            {
                timerText.text = "T-" + detachScript.get_countdown().ToString("00.00") + "s";
            }

            if(detachScript.get_countdown() <= 0)
            {
                SceneManager.LoadScene("EndingDead");
            }
        }
    }

    public void colorUpdate()
    {
        timerText.color = bomb.GetComponent<BombController>().getBombColor();
    }
}