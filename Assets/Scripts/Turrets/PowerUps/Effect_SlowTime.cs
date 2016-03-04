using UnityEngine;
using System.Collections;

public class Effect_SlowTime : MonoBehaviour {

	private float timeScale = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			GameObject playerObj = other.gameObject.transform.parent.gameObject;
			PlayerController playerController = playerObj.GetComponent<PlayerController> ();
			if (!playerController.GetIsSlowed ()) {
				playerController.SlowTime (timeScale);
			}
		} else if (other.gameObject.CompareTag ("Turret")) {
			BulletTurret bulTur = other.gameObject.GetComponent<BulletTurret> ();
			if (bulTur != null && !bulTur.GetIsSlowed ()) {
				bulTur.SlowTime (timeScale);
			}
		} else if (other.gameObject.CompareTag ("Projectile")) {
			//other.gameObject.GetComponent<Bullet> ().SlowTime (timeScale);
			other.gameObject.GetComponent<LightBulletController> ().SlowTime (timeScale);
		} else if (other.gameObject.CompareTag ("Player Projectile")) {
			other.gameObject.GetComponent<PlayerBullet> ().SlowTime (timeScale);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			GameObject playerObj = other.gameObject.transform.parent.gameObject;
			PlayerController playerController = playerObj.GetComponent<PlayerController> ();
			if (playerController.GetIsSlowed ()) {
				playerController.QuickTime (timeScale);
			}
		} else if (other.gameObject.CompareTag ("Turret")) {
			BulletTurret bulTur = other.gameObject.GetComponent<BulletTurret> ();
			if (bulTur != null && bulTur.GetIsSlowed ()) {
				bulTur.QuickTime (timeScale);
			}
		} else if (other.gameObject.CompareTag ("Projectile")) {
			//other.gameObject.GetComponent<Bullet> ().QuickTime (timeScale);
			other.gameObject.GetComponent<LightBulletController> ().QuickTime (timeScale);
		} else if (other.gameObject.CompareTag ("Player Projectile")) {
			other.gameObject.GetComponent<PlayerBullet> ().QuickTime (timeScale);
		}
	}
}
