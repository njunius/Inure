using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorHPDisplayController : MonoBehaviour {

    public ShatterWhenHit doorController;
    public Image doorHPIndicatorBackground;
    private Image doorHPIndicator;
    private HUDColorController colorController;

	// Use this for initialization
	void Start () {
        doorHPIndicator = gameObject.GetComponent<Image>();
        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        doorHPIndicatorBackground.color = colorController.getColorByString("bomb");
        doorHPIndicator.color = colorController.getColorByString("bomb");
        doorHPIndicator.fillAmount = (float)doorController.getHPInternal() / 100f;
	}
	
	// Update is called once per frame
	void Update () {
        if (doorHPIndicator.enabled && doorHPIndicator.fillAmount != (float)doorController.getHPInternal())
        {
            doorHPIndicator.fillAmount = (float)doorController.getHPInternal() / 100f;

        }

        if (doorController != null && doorController.getHPInternal() <= 0)
        {
            doorHPIndicator.enabled = false;
            doorHPIndicatorBackground.enabled = false;
        }
	}

    public void colorUpdate()
    {
        doorHPIndicator.color = colorController.getColorByString("bomb");
        doorHPIndicatorBackground.color = colorController.getColorByString("bomb");
    }
}
