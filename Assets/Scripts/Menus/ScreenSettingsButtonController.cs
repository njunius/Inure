using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScreenSettingsButtonController : MonoBehaviour, IPointerClickHandler {

    private Button thisButton;
    public Button keyButton;
    public Button gameSettingsButton;

    public ResetColorButtonController colorResetButton;

    public TabTransitionController tabTransition;
    private Button[] colorSelectorButtons;

    private Image[] hudElementSamples;

    // Use this for initialization
    void Start () {
        thisButton = gameObject.GetComponent<Button>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Color Selector Button");
        colorSelectorButtons = new Button[temp.Length];

        for (int i = 0; i < colorSelectorButtons.Length; ++i)
        {
            colorSelectorButtons[i] = temp[i].GetComponent<Button>();
        }

        temp = GameObject.FindGameObjectsWithTag("HUD Element Sample");
        hudElementSamples = new Image[temp.Length];

        for (int i = 0; i < hudElementSamples.Length; ++i)
        {
            hudElementSamples[i] = temp[i].GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (thisButton.interactable)
        {
            EventSystem.current.SetSelectedGameObject(null);

            for (int i = 0; i < colorSelectorButtons.Length; ++i)
            {
                colorSelectorButtons[i].interactable = true;
            }

            colorResetButton.resetHUDElementName();

            for (int i = 0; i < hudElementSamples.Length; ++i)
            {
                hudElementSamples[i].enabled = false;
            }

            tabTransition.startTabTransition(gameObject.name);

            thisButton.interactable = false;
            keyButton.interactable = true;
            gameSettingsButton.interactable = true;
        }
    }
}
