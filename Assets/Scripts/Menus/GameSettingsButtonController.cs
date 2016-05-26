using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameSettingsButtonController : MonoBehaviour, IPointerClickHandler {

    private Button thisButton;
    public Button keyButton;
    public Button screenButton;

    public TabTransitionController tabTransition;

    // Use this for initialization
    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (thisButton.interactable)
        {
            EventSystem.current.SetSelectedGameObject(null);

            tabTransition.startTabTransition(gameObject.name);

            thisButton.interactable = false;
            keyButton.interactable = true;
            screenButton.interactable = true;
        }
    }
}

