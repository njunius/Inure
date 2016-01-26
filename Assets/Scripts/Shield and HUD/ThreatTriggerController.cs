using UnityEngine;
using System.Collections;

public class ThreatTriggerController : MonoBehaviour {

    public int numBullets;

	// Use this for initialization
	void Start () {
        numBullets = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void incrementBulletCount()
    {
        ++numBullets;
    }

    public void decrementBulletCount()
    {
        --numBullets;
    }

    public int getNumBullets()
    {
        return numBullets;
    }
}
