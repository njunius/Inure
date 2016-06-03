/*
 * PointTurret.cs
 * 
 * Defines members specific to PointTurret child of SimpleTurret:
 * - Fires single bullet toward player
 * - Highest velocity bullets
 * - Medium rate of fire
 * - No acceleration
 * - Blue bullets
 */

using UnityEngine;
using System.Collections;

public class PointTurret : SimpleTurret {

	// Use this for initialization
	void Start () {
		bulletColor = Color.blue;
		barrelList = new TurretBarrel[1];
		barrelList [0] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
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
				//find new point at end of turret once required to target player
				Vector3 forwardNorm = gameObject.transform.forward;
				forwardNorm.Normalize ();
				endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);
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

        respawnTurretTick();

    }

    /*
	 * Description: Creates new bullet with specifics
	 * Post: new bullet is created with defined color and velocity
	 */
    protected void Fire () {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		CreateBullet (endOfTurret + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		Explode ();
		InvokeRepeating ("DeathBullet", 0f, 0.05f);
	}

	private void DeathBullets () {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 upNorm = gameObject.transform.up;
		upNorm.Normalize ();
		Vector3 rightNorm = gameObject.transform.right;
		rightNorm.Normalize ();

		for (int numBullet = 0; numBullet < 10; ++numBullet) {
			float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			Vector3 aimRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
			randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			aimRotNorm = Vector3.RotateTowards(aimRotNorm, upNorm, randomRads, 0);
			aimRotNorm = aimRotNorm;
			CreateBullet (endOfTurret + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimRotNorm * 2f);
		}
	}

	private void DeathBullet () {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 upNorm = gameObject.transform.up;
		upNorm.Normalize ();
		Vector3 rightNorm = gameObject.transform.right;
		rightNorm.Normalize ();

		float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 aimRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		aimRotNorm = Vector3.RotateTowards(aimRotNorm, upNorm, randomRads, 0);
		CreateBullet (endOfTurret + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimRotNorm * 2f);

		++numDeathBullet;
		if (numDeathBullet == 10) {
			CancelInvoke ("DeathBullet");
		}
	}
}
