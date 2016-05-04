using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ColorSelectorButtonController : MonoBehaviour, IPointerDownHandler {

    private Button thisButton;
    private Button[] colorSelectorButtons;

    // Use this for initialization
    void Start () {
        thisButton = gameObject.GetComponent<Button>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Color Selector Button");
        colorSelectorButtons = new Button[temp.Length];

        for(int i = 0; i < colorSelectorButtons.Length; ++i)
        {
            colorSelectorButtons[i] = temp[i].GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (thisButton.interactable)
        {
            for (int i = 0; i < colorSelectorButtons.Length; ++i)
            {
                colorSelectorButtons[i].interactable = true;
            }
            thisButton.interactable = false;
        }
    }
}
