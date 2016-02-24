using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnTurretsOn : MonoBehaviour {

	public List<GameObject> turretList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			toggleTurrets ();
		}
	}

	void toggleTurrets () {
		for (int numTurret = 0; numTurret < turretList.Count; ++numTurret) {
			turretList [numTurret].GetComponent<Turret> ().TurnOn ();
		}
	}
}
