using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorHPDisplayController : MonoBehaviour {

    public ShatterWhenHit doorController;
    private Image doorHPIndicator;
    private HUDColorController colorController;

	// Use this for initialization
	void Start () {
        doorHPIndicator = gameObject.GetComponent<Image>();
        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        doorHPIndicator.color = colorController.getColorByString("bomb");
        doorHPIndicator.fillAmount = (float)doorController.getHPInternal() / 100f;
	}
	
	// Update is called once per frame
	void Update () {
	    if(doorController != null && doorController.getHPInternal() <= 0)
        {
            doorHPIndicator.enabled = false;
        }
	}

    public void colorUpdate()
    {
        doorHPIndicator.color = colorController.getColorByString("bomb");
    }
}
