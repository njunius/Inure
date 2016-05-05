using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorSliderController : MonoBehaviour {

    private Slider colorChannel;
    private HUDColorController colorController;
    private string hudElement;
    public string colorChannelName;

	// Use this for initialization
	void Start () {
        colorChannel = GetComponent<Slider>();
        hudElement = "";

        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        colorChannel.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ValueChangeCheck()
    {
        if (colorChannelName.Equals("red"))
        {
            colorController.setColorByString(hudElement, colorChannel.value, colorController.getColorByString(hudElement).g, colorController.getColorByString(hudElement).b);

        }
        else if (colorChannelName.Equals("green"))
        {
            colorController.setColorByString(hudElement, colorController.getColorByString(hudElement).r, colorChannel.value, colorController.getColorByString(hudElement).b);

        }
        else if (colorChannelName.Equals("blue"))
        {
            colorController.setColorByString(hudElement, colorController.getColorByString(hudElement).r, colorController.getColorByString(hudElement).g, colorChannel.value);

        }
    }

    public void setHUDElement(string elementName)
    {
        hudElement = elementName;
    }
}
