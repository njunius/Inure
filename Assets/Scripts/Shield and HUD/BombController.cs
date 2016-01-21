/*
 * Controls the physical location of the bomb 
 * Responsible for updating the on screen HUD with the bomb's charge
 * TO BE IMPLEMENTED: moving the bomb off of the player ship and starting a countdown timer
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombController : MonoBehaviour {

    private bool isPlanted;
    public int currBombCharge;
    private int maxBombCharge;

    private GameObject player;
    private Image bombGauge;
    private PlayerController playerBehavior; // for use later when planting the bomb

	// Use this for initialization
	void Start () {

        isPlanted = false;
        currBombCharge = 0;
        maxBombCharge = 100;

        player = GameObject.FindGameObjectWithTag("Player");
        playerBehavior = player.GetComponent<PlayerController>();

        bombGauge = GameObject.FindGameObjectWithTag("Bomb Gauge").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPlanted)
        {
            transform.position = player.transform.position;
        }

        bombGauge.fillAmount = (float)currBombCharge / (float)maxBombCharge;
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

    /*
     * returns true if the bomb has been successfully charged to 100%
     */
    public bool isCharged()
    {
        return currBombCharge == maxBombCharge;
    }
}
