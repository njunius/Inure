using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Effect_EMP : MonoBehaviour {

	private float effectDuration = 5f;
	private bool isPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			if (!isPlayer && other.gameObject.transform.parent.gameObject.GetComponent<PlayerController> ().enabled) {
				other.gameObject.transform.parent.gameObject.GetComponent<PlayerController> ().enabled = false;
			}
		} else if (other.gameObject.CompareTag ("Turret")) {
			if (!other.gameObject.GetComponent<Turret> ().GetIsEMP ()) {
				other.gameObject.GetComponent<Turret> ().EMP ();
			}
		}
	}

	public void IsPlayer () {
		isPlayer = true;
	}
}
