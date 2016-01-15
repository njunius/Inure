﻿using UnityEngine;
using System.Collections;

public class ArcTurret : SimpleTurret {

	private float RELATIVE_SPAWNPOINT_MULTIPLIER = 1.5f;
	private float SEPARATION_ANGLE = Mathf.PI/12;
	private float distFromCenter;

	// Use this for initialization
	void Start () {
		distFromCenter = gameObject.GetComponent<Renderer> ().bounds.extents.z;

		bulletVel = Velocity.High;
		bulletColor = Color.green;
		fireRate = RateOfFire.Medium;
		barrelList = new TurretBarrel[5];
		//leftmost
		barrelList [0] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x * RELATIVE_SPAWNPOINT_MULTIPLIER,
											(int)bulletVel);
		//center-left
		barrelList [1] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x * RELATIVE_SPAWNPOINT_MULTIPLIER,
											(int)bulletVel);
		//center
		barrelList [2] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x * RELATIVE_SPAWNPOINT_MULTIPLIER,
											(int)bulletVel);
		//center-right
		barrelList [3] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x * RELATIVE_SPAWNPOINT_MULTIPLIER,
											(int)bulletVel);
		//leftmost
		barrelList [4] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x * RELATIVE_SPAWNPOINT_MULTIPLIER,
											(int)bulletVel);
	}
	
	// Update is called once per frame
	void Update () {
		var distance = Vector3.Distance (gameObject.transform.position, target.transform.position);
		//if the player is within the turret's range of sight, target the player and fire
		if (distance < sensorRange) {
			gameObject.transform.LookAt (target.transform);
			//find new point at end of turret once required to target player
			Vector3 forwardNorm = gameObject.transform.forward;
			forwardNorm.Normalize ();
			endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * distFromCenter);
			//if not firing, start firing
			if (!isFiring) {
				isFiring = true;
				InvokeRepeating ("fire", (float)fireDelay, (float)fireRate * fireRateMultiplier);
			}
		}
		//if the player is not within range, but the turret is firing, stop firing
		else if (isFiring) {
			isFiring = false;
			CancelInvoke ("fire");
		}
	}

	/*
	 * Description: Shoots five bullets with ArcTurrets specs
	 * Post: A bullet has been fired from all TurretBarrels
	 */
	protected void fire () {
		Vector3 turretCenter = gameObject.GetComponent<Renderer> ().bounds.center;
		Vector3 aimDirNorm = gameObject.transform.forward;
		Vector3 leftNorm = gameObject.transform.right * -1;
		Vector3 rightNorm = gameObject.transform.right;
		aimDirNorm.Normalize ();
		leftNorm.Normalize ();
		rightNorm.Normalize ();

		createBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 0);
		createBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 1);
		createBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 2);
		createBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 3);
		createBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 4);
	}

	/*
	 * Post: A bullet has been created and fired out of one of TurretBarrels 0 through 4 in the proper direction and with the proper velocity
	 * Arguments:
	 * - turrCenter: The center of the ArcTurret GameObject
	 * - aimDirNormMid: The normalized vector denoting the direction in which the middle bullet is to be aimed
	 * - leftNorm: The normalized vector denoting the direction directly leftward from the ArcTurret GameObject
	 * - rightNorm: The normalized vector denoting the direction directly rightward from the ArcTurret GameObject
	 * - bulletNum: The number denoting which of TurretBarrels 0 through 4 is creating a bullet
	 */
	private void createBullet (Vector3 turrCenter, Vector3 aimDirNormMid, Vector3 leftNorm, Vector3 rightNorm, int bulletNum) {
		Vector3 newAimDirNorm = Vector3.zero;
		GameObject bulletObj = null;
		bool debugBool = false;

		switch (bulletNum) {
		case 0:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, leftNorm, SEPARATION_ANGLE*2, 0f);
			bulletObj = (GameObject) Instantiate (bulletPrefab, Vector3.RotateTowards (endOfTurret - turrCenter, leftNorm, SEPARATION_ANGLE*2, 0f) + turrCenter + (newAimDirNorm * (barrelList [0].relativeSpawnPoint)), zQuat);
			break;
		case 1:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, leftNorm, SEPARATION_ANGLE, 0f);
			bulletObj = (GameObject) Instantiate (bulletPrefab, Vector3.RotateTowards (endOfTurret - turrCenter, leftNorm, SEPARATION_ANGLE, 0f) + turrCenter + (newAimDirNorm * (barrelList [1].relativeSpawnPoint)), zQuat);
			break;
		case 2:
			newAimDirNorm = aimDirNormMid;
			bulletObj = (GameObject) Instantiate (bulletPrefab, endOfTurret + (newAimDirNorm * (barrelList [2].relativeSpawnPoint)), zQuat);
			break;
		case 3:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, rightNorm, SEPARATION_ANGLE, 0f);
			bulletObj = (GameObject) Instantiate (bulletPrefab, Vector3.RotateTowards (endOfTurret - turrCenter, rightNorm, SEPARATION_ANGLE, 0f) + turrCenter + (newAimDirNorm * (barrelList [3].relativeSpawnPoint)), zQuat);
			break;
		case 4:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, rightNorm, SEPARATION_ANGLE*2, 0f);
			bulletObj = (GameObject) Instantiate (bulletPrefab, Vector3.RotateTowards (endOfTurret - turrCenter, rightNorm, SEPARATION_ANGLE*2, 0f) + turrCenter + (newAimDirNorm * (barrelList [4].relativeSpawnPoint)), zQuat);
			break;
		default:
			Debug.Log ("Invalid Bullet Number");
			debugBool = true;
			break;
		}
		if (!debugBool) {
			Bullet newBullet = (Bullet)bulletObj.GetComponent (typeof(Bullet));
			newBullet.setVars (bulletColor, newAimDirNorm * (float)bulletVel);
		}
	}
}