using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class KeybindingsButtonController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler {

    private Button thisButton;
    public Button gameSettingsButton;
    public ResetColorButtonController colorResetButton;

    public TabTransitionController tabTransition;
    private Button[] colorSelectorButtons;

    private Image[] hudElementSamples;

    // Use this for initialization
    void Awake () {

        thisButton = gameObject.GetComponent<Button>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Color Selector Button");
        colorSelectorButtons = new Button[temp.Length];

        for (int i = 0; i < colorSelectorButtons.Length; ++i)
        {
            colorSelectorButtons[i] = temp[i].GetComponent<Button>();
        }

        temp = GameObject.FindGameObjectsWithTag("HUD Element Sample");
        hudElementSamples = new Image[temp.Length];

        for(int i = 0; i < hudElementSamples.Length; ++i)
        {
            hudElementSamples[i] = temp[i].GetComponent<Image>();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (thisButton.interactable && !gameSettingsButton.interactable)
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
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (thisButton.interactable && !gameSettingsButton.interactable)
        {
            tabTransition.startTabTransition(true);

            thisButton.interactable = false;
            gameSettingsButton.interactable = true;
        }
    }
}
