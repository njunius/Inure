/*
 * FountainTurret.cs
 * 
 * Defines members specific to FountainTurret child of AlgorithmicTurret:
 * - Fires a variable number of bullets from opposite ends of variable/2
 *   diameter
 * - TurretBarrels swivel inward toward and outward from the center of the
 *   turret as they shoot
 * - Low velocity bullets
 * - High rate of fire
 * - No acceleration
 * - Orange bullets
 */

using UnityEngine;
using System.Collections;

public class FountainTurret : AlgorithmicTurret {
	public int NUM_BARRELS = 4;
	public float ORIG_TURRET_RADIUS = 4;
	public float MAX_SWIVEL = Mathf.PI/3;
	public float SWIVEL_PERCENTAGE = 0.1f;

	private float curTurretRadius;
	private int numFire = 0;
	private bool rotatingOut = true;
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	// Use this for initialization
	void Start () {
		//orange
		bulletColor = new Color(1f, 0.6f, 0f, 1f);
		barrelList = new TurretBarrel[NUM_BARRELS];

		Vector3 forwardNorm = transform.forward;
		forwardNorm.Normalize ();
		endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * 2 * gameObject.GetComponent<Renderer> ().bounds.size.z);

		curTurretRadius = ORIG_TURRET_RADIUS;

		for (int numBarrel = 0; numBarrel < NUM_BARRELS; ++numBarrel) {
			barrelList[numBarrel] = new TurretBarrel ((float)bulletPrefab.GetComponent<Renderer>().bounds.size.x, 
				(int)bulletVel);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isDead && health == 0) {
			Die ();
		}else if (!isEMP) {
			if (isOn) {
				Vector3 forwardNorm = transform.forward;
				forwardNorm.Normalize ();
				endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * 2 * gameObject.GetComponent<Renderer> ().bounds.extents.z);

				//if not firing, start firing
				if (!isFiring) {
					isFiring = true;
					InvokeRepeating ("Fire", fireDelay, fireRate * fireRateMultiplier);
				}

				float distBtwnPlayer = Vector3.Distance (GameObject.FindGameObjectWithTag ("Player").transform.position, transform.position);

				if (distBtwnPlayer < 10f) {
					float percMaxRad = distBtwnPlayer / 10;
					curTurretRadius = Mathf.Max (percMaxRad * ORIG_TURRET_RADIUS, 0f);
				} else {
					curTurretRadius = ORIG_TURRET_RADIUS;
				}
			} else if (isFiring) {
				CancelInvoke ("Fire");
				isFiring = false;
			}
		} else {
			CancelInvoke ("Fire");
			isFiring = false;
		}
	}

	/*
	 * Description: Shoots bullets equally distributed around the turret while the barrels swivel away from and back toward the center
	 * Post: A bullet has been fired from all TurretBarrels at an angle determined by MAX_SWIVEL and SWIVEL_PERCENTAGE
	 */
	protected void Fire() {
		Vector3 aimDirNorm = transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();
		Vector3 upNorm = transform.up;
		upNorm.Normalize ();

		Vector3 bulletPosition = upNorm * curTurretRadius;
		aimDirNorm = Vector3.RotateTowards (aimDirNorm, upNorm, MAX_SWIVEL * SWIVEL_PERCENTAGE * numFire, 0f);

		for (int numBarrel = 0; numBarrel < NUM_BARRELS; ++numBarrel) {
			Quaternion rotQuat = Quaternion.AngleAxis (360f / NUM_BARRELS * numBarrel, transform.forward);
			CreateBullet (endOfTurret + rotQuat * bulletPosition, rotQuat * aimDirNorm);
		}

		if (rotatingOut) {
			++numFire;
			if (numFire + 1 > 1 / SWIVEL_PERCENTAGE) {
				rotatingOut = false;
			}
		} else {
			--numFire;
			if (numFire == 0) {
				rotatingOut = true;
			}
		}

		//rotate the turret by the given angle
		//transform.Rotate (ROTATION_ANGLE);
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		InvokeRepeating ("DeathBullet", 0f, 0.05f);
	}

	private void DeathBullet () {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 upNorm = gameObject.transform.up;
		upNorm.Normalize ();
		Vector3 rightNorm = gameObject.transform.right;
		rightNorm.Normalize ();

		Vector3 bulletPosition = upNorm * curTurretRadius;
		aimDirNorm = Vector3.RotateTowards (aimDirNorm, upNorm, MAX_SWIVEL * SWIVEL_PERCENTAGE * numFire, 0f);

		for (int numBarrel = 0; numBarrel < NUM_BARRELS; ++numBarrel) {
			Quaternion rotQuat = Quaternion.AngleAxis (360f / NUM_BARRELS * numBarrel, transform.forward);
			float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			Vector3 aimRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
			randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			aimRotNorm = Vector3.RotateTowards(aimRotNorm, upNorm, randomRads, 0);
			CreateBullet (endOfTurret + rotQuat * bulletPosition, rotQuat * aimRotNorm * 2f);
		}

		++numDeathBullet;
		if (numDeathBullet == 10) {
			CancelInvoke ("DeathBullet");
		}
	}
}
