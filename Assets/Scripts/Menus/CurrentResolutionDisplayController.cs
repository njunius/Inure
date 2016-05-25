using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurrentResolutionDisplayController : MonoBehaviour {

    private Text resolutionDisplayText;
    private Canvas settingsScreen;

	// Use this for initialization
	void Awake () {
        resolutionDisplayText = GetComponentInChildren<Text>();
        resolutionDisplayText.text = "Current Resolution: " + Screen.currentResolution; ;

        settingsScreen = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        if (settingsScreen.enabled)
        {
            resolutionDisplayText.text = "Current Resolution: " + Screen.currentResolution;
        }
    }

    public void updateDisplayedResolution()
    {
        resolutionDisplayText.text = "Current Resolution: " + Screen.currentResolution;
    }
}
