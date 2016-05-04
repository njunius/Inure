using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameSettingsButtonController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler {

    private GameObject[] keybindingElements;
    private GameObject[] gameSettingElements;
    private Button thisButton;
    public Button keyButton;

    // Use this for initialization
    void Start()
    {

        keybindingElements = GameObject.FindGameObjectsWithTag("Keybinding Screen");

        gameSettingElements = GameObject.FindGameObjectsWithTag("Game Settings Screen");

        thisButton = gameObject.GetComponent<Button>();

        for(int i = 0; i < gameSettingElements.Length; ++i)
        {
            gameSettingElements[i].SetActive(false);
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (thisButton.interactable && !keyButton.interactable)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (thisButton.interactable && !keyButton.interactable)
        {

            for (int i = 0; i < keybindingElements.Length; ++i)
            {
                keybindingElements[i].SetActive(false);
            }

            for (int i = 0; i < gameSettingElements.Length; ++i)
            {
                gameSettingElements[i].SetActive(true);
            }

            thisButton.interactable = false;
            keyButton.interactable = true;
        }
    }
}

