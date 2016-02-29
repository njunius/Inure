using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnTurretsOff : MonoBehaviour {

	GameObject[] allTurrets;

	// Use this for initialization
	void Start () {
		allTurrets = GameObject.FindGameObjectsWithTag ("Turret");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			TurnOff ();
		}
	}

	void TurnOff () {
		for (int numTurret = 0; numTurret < allTurrets.Length; ++numTurret) {
			allTurrets [numTurret].GetComponent<Turret> ().TurnOff ();
		}
	}
}
