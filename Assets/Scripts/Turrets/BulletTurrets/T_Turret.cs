/*
 * T_Turret.cs
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
	public int BULLET_FREQUENCY = 10;
	public int BARREL_SEPARATION = 1;
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	// Use this for initialization
	void Start () {
		bulletColor = Color.cyan;
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
		if (!isDead && health == 0) {
			Die ();
		}
		//if the player is within the turret's range of sight, target the player and fire
		else if (!isEMP) {
			if (isOn) {
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
					InvokeRepeating ("Fire", fireDelay, fireRate * fireRateMultiplier);
				}
			}
			//if the player is not within range, but the turret is firing, stop firing
			else if (isFiring) {
				isFiring = false;
				CancelInvoke ("Fire");
			}
		} else {
			CancelInvoke ("Fire");
			isFiring = false;
		}
	}

	/*
	 * Description: Calls for a burst of bullets
	 * Post: singleBurst() is set to be called repeatedly
	 *       Turret's angle of rotation is increased or reset
	 */
	protected void Fire () {
		InvokeRepeating ("SingleBurst", fireDelay, fireRate * fireRateMultiplier / BULLET_FREQUENCY);

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
	protected void SingleBurst() {
		if (numShots == 4) {
			Vector3 rightNorm = transform.right;
			rightNorm.Normalize ();

			Vector3 aimDirNorm = gameObject.transform.forward;
			aimDirNorm.Normalize ();
			CreateBullet (endOfTurret + (rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);
			CreateBullet (endOfTurret + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), aimDirNorm);
			CreateBullet (endOfTurret + (-1 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);

			CancelInvoke ("SingleBurst");
			numShots = 0;
		}
		else {
			Vector3 aimDirNorm = gameObject.transform.forward;
			aimDirNorm.Normalize ();
			CreateBullet (endOfTurret + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), aimDirNorm);
			++numShots;
		}
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		CancelInvoke ("SingleBurst");
	}
}