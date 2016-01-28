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

	private float BARREL_SEPARATION = 4f;

	// Z-component must be a factor of 360
	private Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	public Vector3 bulletPosition;

	// Use this for initialization
	void Start () {
		bulletVel = Velocity.Low;
		bulletColor = Color.red;
		fireRate = RateOfFire.Extreme;
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

		Vector3 forwardNorm = transform.forward;
		forwardNorm.Normalize ();
		endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);

		//if not firing, start firing
		if (!isFiring) {
			isFiring = true;
			InvokeRepeating ("fire", (float)fireDelay, (float)fireRate * fireRateMultiplier);
		}
	}

	/*
	 * Description: Shoots two bullets in straight, parallel lines
	 * Post: A bullet has been fired from both TurretBarrels
	 */
	protected void fire() {
		Vector3 aimDirNorm = transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();

		//top
		GameObject bulletObj = (GameObject)Instantiate (bulletPrefab, endOfTurret + (-1 * rightNorm * BARREL_SEPARATION / 2) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), zQuat);
		Bullet newBullet = (Bullet)bulletObj.GetComponent (typeof(Bullet));
		newBullet.setVars (bulletColor, aimDirNorm * (float)bulletVel);

		//center-left
		bulletObj = (GameObject)Instantiate (bulletPrefab, endOfTurret + (rightNorm * BARREL_SEPARATION / 2) + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), zQuat);
		newBullet = (Bullet)bulletObj.GetComponent (typeof(Bullet));
		newBullet.setVars (bulletColor, aimDirNorm * (float)bulletVel);

		//rotate the turret by the given angle
		transform.Rotate (ROTATION_ANGLE);
	}
}
