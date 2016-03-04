using UnityEngine;
using System.Collections;

public class Effect_Shockwave : MonoBehaviour {

	private float velMultiplier = 4f;
	private float effectDuration = 2f;
	private bool isPlayer = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Projectile")) {
			if (other.gameObject.GetComponent<Bullet> ()) {
				other.gameObject.GetComponent<Bullet> ().setVars (Color.clear, Vector3.Normalize (other.gameObject.transform.position - gameObject.transform.position) * velMultiplier);
			}

			/*if (other.gameObject.GetComponent<LightBulletController> ()) {
				other.gameObject.GetComponent<LightBulletController> ().setVars (Color.clear, Vector3.Normalize (other.gameObject.transform.position - gameObject.transform.position) * velMultiplier);
			}*/
		} else if (other.gameObject.CompareTag ("Player Projectile")) {
			other.gameObject.GetComponent<PlayerBullet> ().setVars (Color.clear, Vector3.Normalize (other.gameObject.transform.position - gameObject.transform.position) * velMultiplier);
		} else if (!isPlayer && other.gameObject.CompareTag ("Player Turret Trigger")) {
			GameObject playerObj = other.gameObject.transform.parent.gameObject;
			playerObj.GetComponent<PlayerController> ().enabled = false;
			playerObj.GetComponent<Rigidbody> ().velocity = Vector3.Normalize (other.gameObject.transform.position - gameObject.transform.position) * velMultiplier;
			CancelInvoke ("EnablePlayer");
			Invoke ("EnablePlayer", effectDuration);
		}
	}

	private void EnablePlayer () {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().enabled = true;
	}

	public void IsPlayer () {
		isPlayer = true;
	}
}
