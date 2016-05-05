using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ResetColorButtonController : MonoBehaviour, IPointerClickHandler {

    private string hudElementName;
    private HUDColorController colorController;
    private Slider[] colorSlidersForEdit;

	// Use this for initialization
	void Start () {
        hudElementName = "";
        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
