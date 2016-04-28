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
		Explode ();
		for (int numChild = 0; numChild < transform.childCount; ++numChild) {
			transform.GetChild (numChild).gameObject.SetActive (false);
		}
		Instantiate (reward, transform.position + new Vector3 (0, gameObject.GetComponent<Renderer> ().bounds.size.y * 1.5f, 0), transform.rotation);
	}
}
