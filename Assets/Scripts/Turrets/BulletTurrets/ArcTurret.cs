/*
 * ArcTurret.cs
 * 
 * Defines members specific to ArcTurret child of SimpleTurret:
 * - Fires five bullets in arc toward player
 * - High velocity bullets
 * - Medium rate of fire
 * - No acceleration
 * - Green bullets
 */

using UnityEngine;
using System.Collections;

public class ArcTurret : SimpleTurret {

	public float RELATIVE_SPAWNPOINT_MULTIPLIER = 1.5f;
	public float SEPARATION_ANGLE = Mathf.PI/12;
	// Z-component must be a factor of 360
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	private int numFire = 0;
	private float distFromCenter;

	// Use this for initialization
	void Start () {
		distFromCenter = gameObject.GetComponent<Renderer> ().bounds.size.z;

		bulletColor = Color.green;
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
		if (!isDead && health == 0) {
			Die ();
		}
		//if the player is within the turret's range of sight, target the player and fire
		else if (!isEMP) {
			if (isOn) {
				gameObject.transform.LookAt (target.transform);

				//give appropriate rotation for the number of times the turret has fired
				transform.Rotate (ROTATION_ANGLE * numFire);

				//find new point at end of turret once required to target player
				Vector3 forwardNorm = gameObject.transform.forward;
				forwardNorm.Normalize ();
				endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * distFromCenter);
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
	 * Description: Shoots five bullets with ArcTurrets specs
	 * Post: A bullet has been fired from all TurretBarrels
	 */
	protected void Fire () {
		Vector3 turretCenter = gameObject.GetComponent<Renderer> ().bounds.center;
		Vector3 aimDirNorm = gameObject.transform.forward;
		Vector3 leftNorm = gameObject.transform.right * -1;
		Vector3 rightNorm = gameObject.transform.right;
		aimDirNorm.Normalize ();
		leftNorm.Normalize ();
		rightNorm.Normalize ();

		DetermineBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 0);
		DetermineBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 1);
		DetermineBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 2);
		DetermineBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 3);
		DetermineBullet (turretCenter, aimDirNorm, leftNorm, rightNorm, 4);

		//if the turret has made a complete rotation, reset the number of times it has been fired
		if (numFire == 360 / ROTATION_ANGLE.z)
			numFire = 1;
		//else increase the number of times the turret has been fired
		else
			++numFire;
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
	private void DetermineBullet (Vector3 turrCenter, Vector3 aimDirNormMid, Vector3 leftNorm, Vector3 rightNorm, int bulletNum) {
		Vector3 newAimDirNorm = Vector3.zero;
		GameObject bulletObj = null;
		bool debugBool = false;

		switch (bulletNum) {
		case 0:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, leftNorm, SEPARATION_ANGLE * 2, 0f);
			CreateBullet (Vector3.RotateTowards (endOfTurret - turrCenter, leftNorm, SEPARATION_ANGLE*2, 0f) + turrCenter + (newAimDirNorm * (barrelList [0].relativeSpawnPoint)), newAimDirNorm);
			break;
		case 1:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, leftNorm, SEPARATION_ANGLE, 0f);
			CreateBullet (Vector3.RotateTowards (endOfTurret - turrCenter, leftNorm, SEPARATION_ANGLE, 0f) + turrCenter + (newAimDirNorm * (barrelList [1].relativeSpawnPoint)), newAimDirNorm);
			break;
		case 2:
			newAimDirNorm = aimDirNormMid;
			CreateBullet (endOfTurret + (newAimDirNorm * (barrelList [2].relativeSpawnPoint)), newAimDirNorm);
			break;
		case 3:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, rightNorm, SEPARATION_ANGLE, 0f);
			CreateBullet (Vector3.RotateTowards (endOfTurret - turrCenter, rightNorm, SEPARATION_ANGLE, 0f) + turrCenter + (newAimDirNorm * (barrelList [3].relativeSpawnPoint)), newAimDirNorm);
			break;
		case 4:
			newAimDirNorm = Vector3.RotateTowards (aimDirNormMid, rightNorm, SEPARATION_ANGLE*2, 0f);
			CreateBullet (Vector3.RotateTowards (endOfTurret - turrCenter, rightNorm, SEPARATION_ANGLE*2, 0f) + turrCenter + (newAimDirNorm * (barrelList [4].relativeSpawnPoint)), newAimDirNorm);
			break;
		default:
			debugBool = true;
			break;
		}
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		InvokeRepeating ("DeathBullet", 0f, 0.05f);
	}

	private void DeathBullet () {
		Vector3 turretCenter = gameObject.GetComponent<Renderer> ().bounds.center;
		Vector3 aimDirNorm = gameObject.transform.forward;
		Vector3 leftNorm = gameObject.transform.right * -1;
		Vector3 rightNorm = gameObject.transform.right;
		Vector3 upNorm = gameObject.transform.up;
		aimDirNorm.Normalize ();
		leftNorm.Normalize ();
		rightNorm.Normalize ();
		upNorm.Normalize ();

		for (int numBullet = 0; numBullet < 5; ++numBullet) {
			float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			Vector3 aimRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
			randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			aimRotNorm = Vector3.RotateTowards(aimRotNorm, upNorm, randomRads, 0);
			aimRotNorm = aimRotNorm;
			DetermineBullet (turretCenter, aimRotNorm * 2f, leftNorm, rightNorm, numBullet);
		}

		++numDeathBullet;
		if (numDeathBullet == 10) {
			CancelInvoke ("DeathBullet");
		}
	}
}
