using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameSettingsButtonController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler {

    private Button thisButton;
    public Button keyButton;

    public TabTransitionController tabTransition;

    // Use this for initialization
    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();

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
            tabTransition.startTabTransition(false);

            thisButton.interactable = false;
            keyButton.interactable = true;
        }
    }
}

