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
		//if the player is within the turret's range of sight, target the player and fire
		if (isOn) {
			gameObject.transform.LookAt (target.transform);
			//find new point at end of turret once required to target player
			Vector3 forwardNorm = gameObject.transform.forward;
			forwardNorm.Normalize ();
			endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);
			//if not firing, start firing
			if (!isFiring) {
				isFiring = true;
				InvokeRepeating ("fire", fireDelay, fireRate * fireRateMultiplier);
			}
		}
		//if the player is not within range, but the turret is firing, stop firing
		else if (isFiring) {
			isFiring = false;
			CancelInvoke ("fire");
		}
	}

	/*
	 * Description: Creates new bullet with specifics
	 * Post: new bullet is created with defined color and velocity
	 */
	protected void fire () {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		CreateBullet (endOfTurret + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);
	}
}
