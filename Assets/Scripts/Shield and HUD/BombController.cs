using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public bool isPlanted;
    public int currBombCharge;
    public int maxBombCharge;

    public Transform playerLocation;

	// Use this for initialization
	void Start () {

        isPlanted = false;
        currBombCharge = 0;
        maxBombCharge = 100;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPlanted)
        {
            transform.position = playerLocation.position;
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
}
