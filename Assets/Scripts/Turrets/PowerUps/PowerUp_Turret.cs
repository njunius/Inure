using UnityEngine;
using System.Collections;

public class PowerUp_Turret : Turret {

	protected PowerUp powerUp;
	public GameObject reward;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead && health == 0f) {
			Die ();
		} else if (!isFiring && isOn) {
			isFiring = true;
			InvokeRepeating ("Fire", fireDelay, fireRate);
		} else if (!isOn) {
			isFiring = false;
			CancelInvoke ("Fire");
		}
	}

	private void Fire () {
		gameObject.GetComponent<PowerUp> ().Activate ();
	}

	protected void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		Instantiate (reward, transform.position + transform.up, transform.rotation);
	}
}
