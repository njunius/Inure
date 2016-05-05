using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombCountdownController : MonoBehaviour
{

    public GameObject bomb;
    private BombDetach detachScript;
    private Canvas timerTextCanavas;
    private Text timerText;

    // Use this for initialization
    void Start()
    {
        detachScript = bomb.GetComponent<BombDetach>();
        timerTextCanavas = GetComponent<Canvas>();
        timerText = GetComponentInChildren<Text>();

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
        }
    }
}