using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class KeybindingsButtonController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler {

    private GameObject[] keybindingElements;
    private GameObject[] gameSettingElements;
    private Button thisButton;
    public Button gameSettingsButton;

    public TabTransitionController tabTransition;
    private Button[] colorSelectorButtons;



    // Use this for initialization
    void Awake () {

        keybindingElements = GameObject.FindGameObjectsWithTag("Keybinding Screen");

        gameSettingElements = GameObject.FindGameObjectsWithTag("Game Settings Screen");

        thisButton = gameObject.GetComponent<Button>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Color Selector Button");
        colorSelectorButtons = new Button[temp.Length];

        for (int i = 0; i < colorSelectorButtons.Length; ++i)
        {
            colorSelectorButtons[i] = temp[i].GetComponent<Button>();
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
