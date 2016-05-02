﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class KeybindingsButtonController : MonoBehaviour, IPointerDownHandler {

    private GameObject[] keybindingElements;
    private GameObject[] gameSettingElements;
    private Button thisButton;
    public Button gameSettingsButton;

	// Use this for initialization
	void Awake () {

        keybindingElements = GameObject.FindGameObjectsWithTag("Keybinding Screen");

        gameSettingElements = GameObject.FindGameObjectsWithTag("Game Settings Screen");

        thisButton = gameObject.GetComponent<Button>();


	}

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        if (thisButton.interactable && !gameSettingsButton.interactable)
        {
            for (int i = 0; i < keybindingElements.Length; ++i)
            {
                keybindingElements[i].SetActive(true);
            }

            for (int i = 0; i < gameSettingElements.Length; ++i)
            {
                gameSettingElements[i].SetActive(false);
            }

            thisButton.interactable = false;
            gameSettingsButton.interactable = true;
        }
    }
}
