/*
 * Controls the physical location of the bomb 
 * Responsible for updating the on screen HUD with the bomb's charge
 * TO BE IMPLEMENTED: moving the bomb off of the player ship and starting a countdown timer
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombController : MonoBehaviour, HUDElement {

    private bool isArmed;
    private bool hasRigidbody;
    public int currBombCharge;
    private int maxBombCharge;
	private float currUseCharge;

    public BombCountdownController bombTimer;
    public Image bombGaugeBackground;

    private HUDColorController hudColorController;
    private string hudElementName;

    private Color bombHUDColor;
	private Color useHUDColor;

    private DoorHPDisplayController[] doorHPIndicators;
    private ReactorCoreUIController reactorHPIndicator;

    private Image[] bombGauge;
	private Image useGauge;
	private GameObject useGaugeObject;

	// Use this for initialization
	void Start () {

        hudElementName = "bomb";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        isArmed = false;
        hasRigidbody = false;
        currBombCharge = 0;
        maxBombCharge = 100;
		currUseCharge = 0f;

        //transform.parent = player.transform;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Bomb Gauge");
        bombGauge = new Image[temp.Length];
		useGaugeObject = GameObject.FindGameObjectWithTag ("Bomb Use Gauge");

		Color newColor = hudColorController.getColorByString(hudElementName);

        for(int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i] = temp[i].GetComponent<Image>();
			bombHUDColor = bombGauge [i].color = newColor;
        }

        bombGaugeBackground.color = newColor;

		useGauge = useGaugeObject.GetComponent<Image>();
		useGauge.color = new Color (newColor.r / 2, newColor.g / 2, newColor.b / 2, newColor.a);

        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].fillAmount = (float)currBombCharge / (float)maxBombCharge;
        }

        temp = GameObject.FindGameObjectsWithTag("Door HP Indicator");

        doorHPIndicators = new DoorHPDisplayController[temp.Length];

        for(int i = 0; i < doorHPIndicators.Length; ++i)
        {
            doorHPIndicators[i] = temp[i].GetComponent<DoorHPDisplayController>();
        }

        if(GameObject.FindGameObjectWithTag("Reactor Display") != null)
        {
            reactorHPIndicator = GameObject.FindGameObjectWithTag("Reactor Display").GetComponent<ReactorCoreUIController>();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (isArmed && !hasRigidbody)
        {
            transform.parent = null;
            Rigidbody temp = gameObject.AddComponent<Rigidbody>();
            temp.useGravity = false;
            temp.angularDrag = 1;

            hasRigidbody = true;
        }

		setUseRotation (360f * (float)currBombCharge / (float)maxBombCharge);
		useGauge.fillAmount = currUseCharge / (float)maxBombCharge;
    }

    /*
    * pre: newCharge is a positive number
    * post: currBombCharge += newCharge and currBombCharge <= maxBombCharge
    */
    public void chargeBomb(int newCharge)
    {
        currBombCharge += newCharge;
        if (currBombCharge > maxBombCharge)
            currBombCharge = maxBombCharge;

        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].fillAmount = (float)currBombCharge / (float)maxBombCharge;
        }
    }

	public int getBombCharge()
	{
		return currBombCharge;
	}

    // use only when reloading checkpoints
    // charge <= maxBombCharge
    public void setBombCharge(int charge)
    {
        if(charge > maxBombCharge)
        {
            currBombCharge = maxBombCharge;
        }
        else if(charge < 0)
        {
            charge = 0;
        }
        else
        {
            currBombCharge = charge;
        }

        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].fillAmount = (float)currBombCharge / (float)maxBombCharge;
        }
    }

	public float getUseCharge() {
		return currUseCharge;
	}

	public void setUseCharge(float charge) {
		currUseCharge = charge;
	}

	public void setUseRotation(float angle)
	{
		useGaugeObject.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
	}

    /*
     * returns true if the bomb has been successfully charged to 100%
     */
    public bool isCharged()
    {
        return currBombCharge == maxBombCharge;
    }

    public void armBomb()
    {
        isArmed = true;
    }

    public Color getBombColor()
    {
        return bombHUDColor;
    }

    // updates the bomb gauge and other associated HUD elements' colors
    public void UpdateColor()
    {
		Color newColor = hudColorController.getColorByString(hudElementName);
        for (int i = 0; i < bombGauge.Length; ++i)
        {
			bombGauge [i].color = newColor;
        }

        bombGaugeBackground.color = newColor;

		useGauge.color = new Color (newColor.r / 2, newColor.g / 2, newColor.b / 2, newColor.a);

        bombHUDColor = hudColorController.getColorByString(hudElementName);

        bombTimer.colorUpdate();

        for (int i = 0; i < doorHPIndicators.Length; ++i)
        {
            doorHPIndicators[i].colorUpdate(useGauge.color);
        }

        if(reactorHPIndicator != null)
        {
            reactorHPIndicator.colorUpdate();
        }
    }
}
