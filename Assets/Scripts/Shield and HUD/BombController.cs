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

    public BombCountdownController bombTimer;

    private HUDColorController hudColorController;
    private string hudElementName;

    private Color bombHUDColor;

    private Image[] bombGauge;

	// Use this for initialization
	void Start () {

        hudElementName = "bomb";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        isArmed = false;
        hasRigidbody = false;
        currBombCharge = 0;
        maxBombCharge = 100;

        //transform.parent = player.transform;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Bomb Gauge");
        bombGauge = new Image[temp.Length];

        for(int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i] = temp[i].GetComponent<Image>();
            bombHUDColor = bombGauge[i].color = hudColorController.getColorByString(hudElementName);
        }

        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].fillAmount = (float)currBombCharge / (float)maxBombCharge;
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
        currBombCharge = charge;

        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].fillAmount = (float)currBombCharge / (float)maxBombCharge;
        }
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

    public void UpdateColor()
    {
        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].color = hudColorController.getColorByString(hudElementName);
        }

        bombHUDColor = hudColorController.getColorByString(hudElementName);

        bombTimer.colorUpdate();
    }
}
