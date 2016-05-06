using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDSampleColorController : MonoBehaviour {

    public Slider[] colorChannels;
    private Image hudElement;
	// Use this for initialization
	void Start () {
        hudElement = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hudElement.enabled)
        {
            // alpha value is the same as the global color values
            hudElement.color = new Color(colorChannels[0].value, colorChannels[1].value, colorChannels[2].value, 0.784f);
        }
	}
}
