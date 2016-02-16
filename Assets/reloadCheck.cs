using UnityEngine;
using System.Collections;

public class reloadCheck : MonoBehaviour {
	protected LastCheckpoint savedData;
	protected PlayerController pController;
	protected GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		pController = player.GetComponent<PlayerController>();
		savedData = player.GetComponent<LastCheckpoint>();
	}

	public void resetCheckP(){
		pController.reloadCheckP(savedData);
	}

}
