using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ResetColorButtonController : MonoBehaviour, IPointerClickHandler {

    private string hudElementName;
    private HUDColorController colorController;
    private Slider[] colorSlidersForEdit;

    private Button thisButton;
    private Image thisButtonImage;
    private Text thisButtonText;

    private Canvas settingsScreen;

	// Use this for initialization
	void Start () {
        hudElementName = "";
        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        // gets button information to disable when no HUD element is selected
        thisButton = GetComponent<Button>();
        thisButtonImage = GetComponent<Image>();
        thisButtonText = GetComponentInChildren<Text>();

        settingsScreen = GetComponentInParent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(settingsScreen.enabled && hudElementName.Equals(""))
        {
            thisButton.enabled = false;
            thisButtonImage.enabled = false;
            thisButtonText.enabled = false;
        }
        else if(settingsScreen.enabled && !hudElementName.Equals(""))
        {
            thisButton.enabled = true;
            thisButtonImage.enabled = true;
            thisButtonText.enabled = true;
        }
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        // hudElementName should only ever be:
        // shield, armor, powerup, bomb
        if (!hudElementName.Equals(""))
        {
            colorController.setColorToDefault(hudElementName);

            // should only ever be size 3
            colorSlidersForEdit[0].value = colorController.getColorByString(hudElementName).r;
            colorSlidersForEdit[1].value = colorController.getColorByString(hudElementName).g;
            colorSlidersForEdit[2].value = colorController.getColorByString(hudElementName).b;
        }
    }

    public void setHUDElementName(string newElementName, Slider[] colorSliders)
    {
        hudElementName = newElementName;
        colorSlidersForEdit = colorSliders;
    }

    public void resetHUDElementName()
    {
        hudElementName = "";
    }
}
