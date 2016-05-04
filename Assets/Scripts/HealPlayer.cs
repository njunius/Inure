using UnityEngine;
using System.Collections;

public class HealPlayer : MonoBehaviour {

	public GameObject healingParticles;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player Collider")) {
			PlayerController player = other.gameObject.transform.parent.gameObject.GetComponent<PlayerController> ();
			if (player.getMaxHullIntegrity () > player.getCurrHullIntegrity ()) {
				player.restoreHullPoint ();
				GameObject particles = (GameObject)Instantiate (healingParticles);
				Transform playerTransform = player.gameObject.transform;
				particles.transform.parent = playerTransform;
				particles.transform.position = playerTransform.position;
				particles.transform.rotation = playerTransform.rotation;
				Destroy (gameObject);
			}
		}
	}
}
