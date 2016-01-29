﻿/*
 * PointTurret.cs
 * 
 * Defines members specific to T-Turret child of SimpleTurret:
 * - Fires seven bullets in 'T' shape toward player
 * - Highest velocity bullets
 * - High rate of fire
 * - No acceleration
 * - Cyan bullets
 */

using UnityEngine;
using System.Collections;

public class T_Turret : SimpleTurret {

	private int numShots = 0;
	private int numFire = 0;
	private int BULLET_FREQUENCY = 200;
	private int BARREL_SEPARATION = 1;
	private Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	// Use this for initialization
	void Start () {
		bulletVel = Velocity.Extreme;
		bulletColor = Color.cyan;
		fireRate = RateOfFire.High;
		barrelList = new TurretBarrel[3];

		//left
		barrelList [0] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//center
		barrelList [1] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//right
		barrelList [2] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
	}
	
	// Update is called once per frame
	void Update () {
		var distance = Vector3.Distance (gameObject.transform.position, target.transform.position);
		//if the player is within the turret's range of sight, target the player and fire
		if (distance < sensorRange) {
			if (numShots == 0) {
				gameObject.transform.LookAt (target.transform);
				//give appropriate rotation for the number of times the turret has fired
				transform.Rotate (ROTATION_ANGLE * numFire);

				//find new point at end of turret once required to target player
				Vector3 forwardNorm = gameObject.transform.forward;
				forwardNorm.Normalize ();
				endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);
			}
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
	 * Description: Calls for a burst of bullets
	 * Post: singleBurst() is set to be called repeatedly
	 *       Turret's angle of rotation is increased or reset
	 */
	protected void fire () {
		InvokeRepeating ("singleBurst", (float)fireDelay, (float)fireRate / BULLET_FREQUENCY);

		//if the turret has made a complete rotation, reset the number of times it has been fired
		if (numFire == 360 / ROTATION_ANGLE.z)
			numFire = 1;
		//else increase the number of times the turret has been fired
		else
			++numFire;
	}

	/*
	 * Description: Creates a certain number of bullets depending on which part of the 'T' is to be fired
	 * Pre: numShots is within the set {0, ..., 4}
	 * Post: If numShots is less than 4, one bullet is fired from the central barrel
	 *       If numShots is equal to 4, one bullet is fired from each barrel, and numshots is reset
	 */
	protected void singleBurst() {
		if (numShots == 4) {
			Vector3 rightNorm = transform.right;
			rightNorm.Normalize ();

			Vector3 aimDirNorm = gameObject.transform.forward;
			aimDirNorm.Normalize ();
			GameObject bulletObj = (GameObject)Instantiate (bulletPrefab, endOfTurret + (rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), zQuat);
			Bullet newBullet = (Bullet)bulletObj.GetComponent(typeof(Bullet));
			newBullet.setVars (bulletColor, aimDirNorm * (float)bulletVel);
			bulletObj = (GameObject) Instantiate (bulletPrefab, endOfTurret + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), zQuat);
			newBullet = (Bullet)bulletObj.GetComponent(typeof(Bullet));
			newBullet.setVars (bulletColor, aimDirNorm * (float)bulletVel);
			bulletObj = (GameObject)Instantiate (bulletPrefab, endOfTurret + (-1 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), zQuat);
			newBullet = (Bullet)bulletObj.GetComponent(typeof(Bullet));
			newBullet.setVars (bulletColor, aimDirNorm * (float)bulletVel);

			CancelInvoke ("singleBurst");
			numShots = 0;
		}
		else {
			Vector3 aimDirNorm = gameObject.transform.forward;
			aimDirNorm.Normalize ();
			GameObject bulletObj = (GameObject) Instantiate (bulletPrefab, endOfTurret + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), zQuat);
			Bullet newBullet = (Bullet)bulletObj.GetComponent(typeof(Bullet));
			newBullet.setVars (bulletColor, aimDirNorm * (float)bulletVel);
			++numShots;
		}
	}
}