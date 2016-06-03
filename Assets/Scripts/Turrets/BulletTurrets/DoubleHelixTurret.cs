/*
 * DoubleHelixTurret.cs
 * 
 * Defines members specific to DoubleHelixTurret child of AlgorithmicTurret:
 * - Fires two bullets from opposite ends of a diameter as the turret
 *   rotates, forming a double helix
 * - Low velocity bullets
 * - Extremely high rate of fire
 * - No acceleration
 * - Red bullets
 */

using UnityEngine;
using System.Collections;

public class DoubleHelixTurret : AlgorithmicTurret {

	public float BARREL_SEPARATION = 4f;

	// Z-component must be a factor of 360
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	public Vector3 bulletPosition;

	// Use this for initialization
	void Start () {
		bulletColor = Color.red;
		barrelList = new TurretBarrel[2];
		Vector3 forwardNorm = transform.forward;
		forwardNorm.Normalize ();
		endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);

		//left
		barrelList [0] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
		//right
		barrelList [1] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
			(int)bulletVel);
	}

	// Update is called once per frame
	void Update () {
		if (!isDead && health == 0) {
			Die ();
		} else if (!isEMP) {
			if (isOn) {
				Vector3 forwardNorm = transform.forward;
				forwardNorm.Normalize ();
				endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);

				//if not firing, start firing
				if (!isFiring) {
					isFiring = true;
					InvokeRepeating ("Fire", fireDelay, fireRate * fireRateMultiplier);
				}
			} else if (isFiring) {
				CancelInvoke ("Fire");
				isFiring = false;
			}
		} else {
			CancelInvoke ("Fire");
			isFiring = false;
		}

        respawnTurretTick();

    }

    /*
	 * Description: Shoots two bullets in straight, parallel lines
	 * Post: A bullet has been fired from both TurretBarrels
	 */
    protected void Fire() {
		Vector3 aimDirNorm = transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();

		CreateBullet (endOfTurret + (-1 * rightNorm * BARREL_SEPARATION / 2) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);
		CreateBullet (endOfTurret + (rightNorm * BARREL_SEPARATION / 2) + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), aimDirNorm);

		//rotate the turret by the given angle
		transform.Rotate (ROTATION_ANGLE);
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		Explode ();
		InvokeRepeating ("DeathBullet", 0f, 0.05f);
	}

	private void DeathBullet () {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 upNorm = gameObject.transform.up;
		upNorm.Normalize ();
		Vector3 rightNorm = gameObject.transform.right;
		rightNorm.Normalize ();

		float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 leftRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		leftRotNorm = Vector3.RotateTowards(leftRotNorm, upNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 rightRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		rightRotNorm = Vector3.RotateTowards(rightRotNorm, upNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		CreateBullet (endOfTurret + (-1 * rightNorm * BARREL_SEPARATION / 2) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), leftRotNorm * 2f);
		CreateBullet (endOfTurret + (rightNorm * BARREL_SEPARATION / 2) + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), rightRotNorm * 2f);

		++numDeathBullet;
		if (numDeathBullet == 10) {
			CancelInvoke ("DeathBullet");
		}
	}
}
