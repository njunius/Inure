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
    Vector3 pre_position;
    public float sensorRange = 50f;
    public GameObject target;

    private GameObject player;
    private Image[] bombGauge;
    private PlayerController playerBehavior; // for use later when planting the bomb

	// Use this for initialization
	void Start () {

        isPlanted = false;
        currBombCharge = 0;
        maxBombCharge = 100;

        player = GameObject.FindGameObjectWithTag("Player");
        playerBehavior = player.GetComponent<PlayerController>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Bomb Gauge");
        bombGauge = new Image[temp.Length];

        for(int i = 0; i < bombGauge.Length; ++i)
        {
            bombGauge[i] = temp[i].GetComponent<Image>();
        }
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = player.transform.position;

        /*if (distance < sensorRange)
        {
            isPlanted = true;
        }*/

        if (isPlanted)
        {
            //transform.parent = null;
            Debug.Log("Bomb is PLANTED!");
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

    /*
     * returns true if the bomb has been successfully charged to 100%
     */
    public bool isCharged()
    {
        return currBombCharge == maxBombCharge;
    }
}
