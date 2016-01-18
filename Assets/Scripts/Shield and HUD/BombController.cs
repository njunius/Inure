using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombController : MonoBehaviour {

    private bool isPlanted;
    public int currBombCharge;
    private int maxBombCharge;

    public GameObject player;
    public Image bombGauge;
    private PlayerController playerBehavior;

	// Use this for initialization
	void Start () {

        isPlanted = false;
        currBombCharge = 0;
        maxBombCharge = 100;

        playerBehavior = player.GetComponent<PlayerController>();
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
}
