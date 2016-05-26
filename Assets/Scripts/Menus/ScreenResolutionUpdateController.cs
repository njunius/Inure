using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScreenResolutionUpdateController : MonoBehaviour, IPointerClickHandler {

    public int screenWidth;
    public int screenHeight;
    public CurrentResolutionDisplayController currentResolutionText;

    private Text buttonText;

	// Use this for initialization
	void Start () {
        buttonText = gameObject.GetComponentInChildren<Text>();
        buttonText.text = screenWidth + " x " + screenHeight;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreen);
        EventSystem.current.SetSelectedGameObject(null);
        currentResolutionText.updateDisplayedResolution();
    }
}
