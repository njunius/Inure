using UnityEngine;
using System.Collections;

public class Equip_PowerUp : MonoBehaviour {

	protected enum PowerUpVal {None = 0, EMP = 1, Shockwave = 2, SlowTime = 3};
	protected PowerUpVal whichPowerUp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			other.gameObject.transform.parent.gameObject.GetComponent<PlayerController> ().EquipPowerUp ((int)whichPowerUp);
			Destroy (gameObject);
		}
	}
}
