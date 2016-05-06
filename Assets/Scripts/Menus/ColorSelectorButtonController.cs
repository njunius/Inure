using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ColorSelectorButtonController : MonoBehaviour, IPointerDownHandler {

    private Button thisButton;
    private Button[] colorSelectorButtons;

    public string hudElement;
    private Color hudElementColor;
    private HUDColorController colorController;

    public Slider[] colorSliders;
    public ResetColorButtonController resetButton;
    public GameObject[] otherElementSliders;

    public Image hudElementSample;
    public Image[] otherHUDElements;

    // Use this for initialization
    void Start () {
        thisButton = gameObject.GetComponent<Button>();

        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Color Selector Button");
        colorSelectorButtons = new Button[temp.Length];

        for(int i = 0; i < colorSelectorButtons.Length; ++i)
        {
            colorSelectorButtons[i] = temp[i].GetComponent<Button>();
        }

        hudElementColor = colorController.getColorByString(hudElement);
        hudElementSample.color = hudElementColor;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (thisButton.interactable)
        {
            hudElementColor = colorController.getColorByString(hudElement);

            for (int i = 0; i < colorSelectorButtons.Length; ++i)
            {
                colorSelectorButtons[i].interactable = true;
            }
            thisButton.interactable = false;

            for (int i = 0; i < otherElementSliders.Length; ++i)
            {
                otherElementSliders[i].SetActive(false);
            }

            for(int i = 0; i < colorSliders.Length; ++i)
            {
                colorSliders[i].GetComponent<ColorSliderController>().setHUDElement(hudElement);
            }

            hudElementSample.enabled = true;
            hudElementSample.color = hudElementColor;

            for (int i = 0; i < otherHUDElements.Length; ++i)
            {
                otherHUDElements[i].enabled = false;
            }

            // turn on the layout group through the sliders
            colorSliders[0].gameObject.transform.parent.gameObject.SetActive(true);

            // color sliders should only ever be length 3
            colorSliders[0].value = hudElementColor.r;
            colorSliders[1].value = hudElementColor.g;
            colorSliders[2].value = hudElementColor.b;

            resetButton.setHUDElementName(hudElement, colorSliders);
        }
    }

    public void disableSample()
    {
        hudElementSample.enabled = false;
    }
}
