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
	private GameObject explosion;
	private ParticleSystem lastParticle;

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

		if (explosion.activeSelf && !lastParticle.IsAlive ()) {
			Destroy (gameObject);
		}
	}

	public void CalculateVariables (float percOfChargeUsed, Vector3 directionNorm) {
		explosion = transform.FindChild ("Explosion").gameObject;
		lastParticle = explosion.transform.FindChild("Dust").GetComponent<ParticleSystem> ();

		curVel = (MAX_VEL - MIN_VEL) * (percOfChargeUsed/100f) + MIN_VEL;
		deceleration = curVel;
		direction = directionNorm;

		powerLevel = percOfChargeUsed;
		explosionSize = (MAX_EXPLOSION_SIZE - MIN_EXPLOSION_SIZE) * (percOfChargeUsed/100f) + MIN_EXPLOSION_SIZE;
		timeLeft = (MAX_TIME_LEFT - MIN_TIME_LEFT) * (percOfChargeUsed/100f) + MIN_TIME_LEFT;
		Invoke ("StopBombParticles", timeLeft);
		isTriggered = true;
		//Vector3 particleScale = new Vector3 (percOfChargeUsed / 100, percOfChargeUsed / 100, percOfChargeUsed / 100);
		for (int numChild = 0; numChild < explosion.transform.childCount; ++numChild) {
			explosion.transform.GetChild (numChild).GetComponent<ParticleSystem> ().startSize *= (percOfChargeUsed / 25);
		}
		//explosion.transform.localScale = new Vector3(percOfChargeUsed/100, percOfChargeUsed/100, percOfChargeUsed/100);
	}

	private void StopBombParticles() {
		transform.FindChild ("Bomb Particles").GetComponent<ParticleSystem> ().Stop ();
	}

	private void Explode () {
		Transform fieldTrans = transform.FindChild ("Explosive Field");
		fieldTrans.parent = null;
		fieldTrans.GetComponent<PowerBombExplosion> ().SetPowerLevel(powerLevel);
		fieldTrans.gameObject.SetActive (true);
		fieldTrans.GetComponent<PowerBombExplosion> ().Explode (explosionSize);
		transform.FindChild ("Explosion").gameObject.SetActive (true);
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