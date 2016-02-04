/*
 * TriangleTurret_Flat.cs
 * 
 * Defines members specific to ArcTurret child of SimpleTurret:
 * - Fires six bullets in flat (horizontal) triangle shape toward player
 * - Low velocity bullets
 * - Medium rate of fire
 * - No acceleration
 * - Magenta bullets
 */

using UnityEngine;
using System.Collections;

public class TriangleTurret_Flat : SimpleTurret {

	private int numShots = 0;
	private int numFire = 0;
	private int BULLET_FREQUENCY = 150;
	//multiplied by normalized vector on the local 
	private float BARREL_SEPARATION = 0.25f;
	private Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	// Use this for initialization
	void Start () {
		bulletVel = Velocity.Low;
		bulletColor = Color.magenta;
		fireRate = RateOfFire.Medium;
		barrelList = new TurretBarrel[5];

		//leftmost
		barrelList [0] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//center-left
		barrelList [1] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//center
		barrelList [2] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//center-right
		barrelList [3] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//rightmost
		barrelList [4] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
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
	 * Description: Creates a certain number of bullets depending on which part of the triangle is to be fired
	 * Pre: numShots is within the set {0, 1, 2}
	 * Post: If numShots is equal to 0, one bullet is fired from the central barrel
	 *       If numShots is equal to 1, one bullet is fired from each barrel directly beside the central barrel
	 *       If numShots is equal to 2, one bullet is fired from each of the end barrels, and numShots is reset
	 */
	protected void singleBurst() {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();
		GameObject bulletObj;
		Bullet newBullet;

		switch (numShots) {
		case 0:
			CreateBullet (endOfTurret + (aimDirNorm * (barrelList [2].relativeSpawnPoint)), aimDirNorm);
			++numShots;

			break;
		case 1:
			CreateBullet (endOfTurret + (rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), aimDirNorm);
			CreateBullet (endOfTurret + (-1 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [3].relativeSpawnPoint)), aimDirNorm);
			++numShots;

			break;
		case 2:
			CreateBullet (endOfTurret + (2 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);
			CreateBullet (endOfTurret + (aimDirNorm * (barrelList [2].relativeSpawnPoint)), aimDirNorm);
			CreateBullet (endOfTurret + (-2 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [4].relativeSpawnPoint)), aimDirNorm);
			CancelInvoke ("singleBurst");
			numShots = 0;

			break;
		default:
			break;
		}
	}
}
