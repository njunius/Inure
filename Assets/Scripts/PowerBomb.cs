using UnityEngine;
using System.Collections;

public class PowerBomb : MonoBehaviour {

	private float MIN_VEL = 10f;
	private float MAX_VEL = 300f;

	private float MIN_EXPLOSION_SIZE = 10f;
	private float MAX_EXPLOSION_SIZE = 100f;

	private float MIN_TIME_LEFT = 0.5f;
	private float MAX_TIME_LEFT = 3f;

	private float powerLevel;
	private float curVel;
	private float deceleration;
	private Vector3 direction;
	private float explosionSize;
	private float timeLeft;
	private bool isTriggered = false;
	private bool printed = false;

	// Use this for initialization
	void Start () {
		//Use powerLevel to calculate destructive force (size of explosion)
		//   and startVelocity



	}
	
	// Update is called once per frame
	void Update () {
		//Over time, deplete velocity to 0 and tick down to explosion

		if (isTriggered && curVel != 0f) {
			curVel = Mathf.Max (curVel - (deceleration * Time.deltaTime), 0);
			GetComponent<Rigidbody> ().velocity = curVel * direction;
		} else if (curVel == 0f && timeLeft > 0f) {
			timeLeft = Mathf.Max(timeLeft - (1f * Time.deltaTime), 0f);
			if (timeLeft == 0f) {
				//timeLeft = -1f;
				Explode ();
			}
		}
	}

	public void CalculateVariables (float percOfChargeUsed, Vector3 directionNorm) {
		curVel = (MAX_VEL - MIN_VEL) * (percOfChargeUsed/100f) + MIN_VEL;
		deceleration = curVel;
		direction = directionNorm;

		powerLevel = percOfChargeUsed/2;
		explosionSize = (MAX_EXPLOSION_SIZE - MIN_EXPLOSION_SIZE) * (percOfChargeUsed/100f) + MIN_EXPLOSION_SIZE;
		timeLeft = (MAX_TIME_LEFT - MIN_TIME_LEFT) * (percOfChargeUsed/100f) + MIN_TIME_LEFT;
		isTriggered = true;
	}

	private void Explode () {
		transform.GetChild (0).gameObject.SetActive (true);
		gameObject.GetComponentInChildren<PowerBombExplosion> ().Explode (explosionSize);
	}

	public float GetPowerLevel () {
		return powerLevel;
	}
}

/*
 * Restricts linear progression for the sake of challenge
 * Increases relevance of bomb for sake of bomb notoriety and player agency
 * Forces increase of affectiveness of turrets with respect to creation of difficulty
 */