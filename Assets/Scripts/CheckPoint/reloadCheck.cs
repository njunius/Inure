using UnityEngine;
using System.Collections;

public class reloadCheck : MonoBehaviour {
	protected LastCheckpoint savedData;
	protected PlayerController pController;
	protected GameObject player;

    public GameObject[] waypoints;
    public GameObject[] GDLasers;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		pController = player.GetComponent<PlayerController>();
		savedData = player.GetComponent<LastCheckpoint>();
	}

	//Calls the player controller to reload the save
	public void resetCheckP(){
        foreach (GameObject w in waypoints)
        {
            w.GetComponent<Waypoint>().active = false;
        }
        foreach (GameObject g in GDLasers)
        {
            g.GetComponent<GiantDeathLaserOfDoom>().reset();
        }
		pController.reloadCheckP(savedData);
	}

}
