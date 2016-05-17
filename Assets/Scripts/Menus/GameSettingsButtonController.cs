using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameSettingsButtonController : MonoBehaviour, IPointerClickHandler {

    private Button thisButton;
    public Button keyButton;

    public TabTransitionController tabTransition;

    // Use this for initialization
    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (thisButton.interactable && !keyButton.interactable)
        {
            EventSystem.current.SetSelectedGameObject(null);

            tabTransition.startTabTransition(false);

            thisButton.interactable = false;
            keyButton.interactable = true;
        }
    }
}

