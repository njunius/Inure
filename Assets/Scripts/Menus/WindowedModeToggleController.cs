using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WindowedModeToggleController : MonoBehaviour, IPointerClickHandler {

    private Text fullScreenText;

	// Use this for initialization
	void Start () {

        fullScreenText = GetComponentInChildren<Text>();

        if (Screen.fullScreen)
        {
            fullScreenText.text = "Fullscreen";
        }
        else
        {
            fullScreenText.text = "Windowed";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            fullScreenText.text = "Windowed";
        }
        else
        {
            Screen.fullScreen = true;
            fullScreenText.text = "Fullscreen";
        }
    }

}
