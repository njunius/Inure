﻿/*
 * Controls the physical location of the bomb 
 * Responsible for updating the on screen HUD with the bomb's charge
 * TO BE IMPLEMENTED: moving the bomb off of the player ship and starting a countdown timer
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombController : MonoBehaviour {

    private bool isArmed;
    public int currBombCharge;
    private int maxBombCharge;

    private GameObject player;
    private Image[] bombGauge;

	// Use this for initialization
	void Start () {

        isArmed = false;
        currBombCharge = 0;
        maxBombCharge = 100;

        player = GameObject.FindGameObjectWithTag("Player");

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Bomb Gauge");
        bombGauge = new Image[temp.Length];

        for(int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i] = temp[i].GetComponent<Image>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isArmed)
        {
            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;
        }

        for (int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i].fillAmount = (float)currBombCharge / (float)maxBombCharge;
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
    }

    // use only when reloading checkpoints
    public void setBombCharge(int charge)
    {
        currBombCharge = charge;
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
}
