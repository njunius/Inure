using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnLightsOn : MonoBehaviour {
	public string tag = "None";

	private GameObject[] lightList;

	// Use this for initialization
	void Start () {
		lightList = GameObject.FindGameObjectsWithTag (tag);
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
		for (int numLight = 0; numLight < lightList.Length; ++numLight) {
			lightList [numLight].GetComponent<Light> ().enabled = true;
		}
	}
}
