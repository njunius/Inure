using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorHPDisplayController : MonoBehaviour {

    public ShatterWhenHit doorController;
    public Image doorHPIndicatorBackground;
    public Image doorHPIndicatorColorBalancer;
    public Canvas doorHPCanvas;
    private Image doorHPIndicator;
    private HUDColorController colorController;

	// Use this for initialization
	void Start () {
        doorHPIndicator = gameObject.GetComponent<Image>();
        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        Color tempColor = colorController.getColorByString("bomb");

        doorHPIndicatorBackground.color = colorController.getColorByString("bomb");
        // door hp color should be the same as bombUseGauge color
        doorHPIndicator.color = new Color(tempColor.r / 2, tempColor.g / 2, tempColor.b / 2, tempColor.a);
        doorHPIndicator.fillAmount = (float)doorController.getHPInternal() / 100f;

        doorHPIndicatorColorBalancer.color = tempColor;
        doorHPIndicatorColorBalancer.fillAmount = (float)doorController.getHPInternal() / 100f; 
    }
	
	// Update is called once per frame
	void Update () {
        if (doorHPIndicator.enabled && doorHPIndicator.fillAmount != (float)doorController.getHPInternal())
        {
            doorHPIndicator.fillAmount = (float)doorController.getHPInternal() / 100f;
            doorHPIndicatorColorBalancer.fillAmount = (float)doorController.getHPInternal() / 100f;

        }

        if (doorController != null && doorController.getHPInternal() <= 0)
        {
            doorHPCanvas.enabled = false;
        }
	}

    public void colorUpdate(Color newColor)
    {
        // door HP color should be the same as the bombUseGauge color
        doorHPIndicator.color = newColor;
        doorHPIndicatorColorBalancer.color = colorController.getColorByString("bomb");
        doorHPIndicatorBackground.color = colorController.getColorByString("bomb");
    }
}
