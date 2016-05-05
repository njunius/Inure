using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameSettingsButtonController : MonoBehaviour, IPointerDownHandler {

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

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
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

