/*
 * TriangleTurret_Wall.cs
 * 
 * Defines members specific to ArcTurret child of SimpleTurret:
 * - Fires six bullets in wall-like (vertical) triangle shape toward player
 * - Low velocity bullets
 * - Medium rate of fire
 * - No acceleration
 * - Magenta bullets
 */

using UnityEngine;
using System.Collections;

public class TriangleWallLight : SimpleTurret {

	private int numFire = 0;
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	//angle between the rays from barrel 0 to barrel 3 and from barrel 0 to barrel 5
	public const float TRIANGLE_ANGLE = Mathf.PI / 3f;
	public float TRIANGLE_HEIGHT;

	// Use this for initialization
	void Start () {
		bulletVel = Velocity.Low;
		bulletColor = Color.magenta;
		fireRate = RateOfFire.Medium;
		barrelList = new TurretBarrel[6];
		TRIANGLE_HEIGHT = 3f * (float)bulletPrefab.GetComponent<Light>().range;

		//top
		barrelList [0] = new TurretBarrel ((float)bulletPrefab.GetComponent<Light>().range, 
			(int)bulletVel);
		//center-left
		barrelList [1] = new TurretBarrel ((float)bulletPrefab.GetComponent<Light>().range, 
			(int)bulletVel);
		//center-right
		barrelList [2] = new TurretBarrel ((float)bulletPrefab.GetComponent<Light>().range, 
			(int)bulletVel);
		//back-left
		barrelList [3] = new TurretBarrel ((float)bulletPrefab.GetComponent<Light>().range, 
			(int)bulletVel);
		//bottom-center
		barrelList [4] = new TurretBarrel ((float)bulletPrefab.GetComponent<Light>().range, 
			(int)bulletVel);
		//bottom-right
		barrelList [5] = new TurretBarrel ((float)bulletPrefab.GetComponent<Light>().range, 
			(int)bulletVel);
	}

	// Update is called once per frame
	void Update () {
		var distance = Vector3.Distance (gameObject.transform.position, target.transform.position);
		//if the player is within the turret's range of sight, target the player and fire
		if (distance < sensorRange) {
			gameObject.transform.LookAt (target.transform);

			//give appropriate rotation for the number of times the turret has fired
			transform.Rotate (ROTATION_ANGLE * numFire);

			//find new point at end of turret once required to target player
			Vector3 forwardNorm = gameObject.transform.forward;
			forwardNorm.Normalize ();
			endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);
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
	 * Description: Shoots six bullets in a triangle shape
	 * Post: A bullet has been fired from all TurretBarrels
	 */
	protected void fire() {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();
		Vector3 upNorm = transform.up;
		upNorm.Normalize ();

		//distance between barrels 1 and 2 (second row of barrels) - same as half width of base of triangle
		float middleDist = Mathf.Tan(TRIANGLE_ANGLE / 2) * TRIANGLE_HEIGHT;

		CreateBullet (endOfTurret + (TRIANGLE_HEIGHT / 2 * upNorm) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), aimDirNorm);
		CreateBullet (endOfTurret + (-1 * middleDist / 2 * rightNorm) + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), aimDirNorm);
		CreateBullet (endOfTurret + (middleDist / 2 * rightNorm) + (aimDirNorm * (barrelList [2].relativeSpawnPoint)), aimDirNorm);
		CreateBullet (endOfTurret + (-1 * middleDist * rightNorm) + (-1 * TRIANGLE_HEIGHT / 2 * upNorm) + (aimDirNorm * (barrelList [3].relativeSpawnPoint)), aimDirNorm);
		CreateBullet (endOfTurret + (-1 * TRIANGLE_HEIGHT / 2 * upNorm) + (aimDirNorm * (barrelList [4].relativeSpawnPoint)), aimDirNorm);
		CreateBullet (endOfTurret + (middleDist * rightNorm) + (-1 * TRIANGLE_HEIGHT / 2 * upNorm) + (aimDirNorm * (barrelList [5].relativeSpawnPoint)), aimDirNorm);

		//if the turret has made a complete rotation, reset the number of times it has been fired
		if (numFire == 360 / ROTATION_ANGLE.z)
			numFire = 1;
		//else increase the number of times the turret has been fired
		else
			++numFire;
	}


}
