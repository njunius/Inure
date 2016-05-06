using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ColorSliderController : MonoBehaviour {

    private Slider colorChannel;
    private HUDColorController colorController;
    private string hudElement;
    public string colorChannelName;

    public Image elementSample;
    private HUDElement elementToUpdate;

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
        if(elementToUpdate != null)
        {
            elementToUpdate.UpdateColor();
        }

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

        if (!SceneManager.GetActiveScene().name.Contains("Menu"))
        {
            if (hudElement.Equals("armor"))
            {
                elementToUpdate = GameObject.Find("Armor Gauge Canvas").GetComponent<HUDElement>();
            }
            else if (hudElement.Equals("shield"))
            {
                elementToUpdate = GameObject.Find("Shield").GetComponent<HUDElement>();
            }
            else if (hudElement.Equals("powerup"))
            {
                elementToUpdate = GameObject.Find("Powerup Gauge Canvas").GetComponent<HUDElement>();
            }
            else if (hudElement.Equals("bomb"))
            {
                elementToUpdate = GameObject.FindGameObjectWithTag("Bomb").GetComponent<HUDElement>();
            }
        }
    }
}
