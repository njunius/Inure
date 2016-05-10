using UnityEngine;
using System.Collections;

public class PowerBomb : MonoBehaviour {

	private float MAX_VEL = 300;
	private float MIN_VEL = 10;

	private float powerLevel;
	private float curVel;
	private float deceleration;
	private Vector3 direction;
	private float sizeOfExplosion;
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

		if (isTriggered && curVel != 0) {
			curVel = Mathf.Max (curVel - (deceleration * Time.deltaTime), 0);
			GetComponent<Rigidbody> ().velocity = curVel * direction;
		} else if (curVel == 0 && !printed) {
			printed = true;
			Debug.Log ("The bomb has stopped");
		}
	}

	public void CalculateVariables (float percOfChargeUsed, Vector3 directionNorm) {
		curVel = (MAX_VEL - MIN_VEL) * (percOfChargeUsed/100) + MIN_VEL;
		deceleration = curVel;
		direction = directionNorm;

		isTriggered = true;
	}
}

/*
 * Restricts linear progression for the sake of challenge
 * Increases relevance of bomb for sake of bomb notoriety and player agency
 * Forces increase of affectiveness of turrets with respect to creation of difficulty
 */