/*
 * RakingTurret.cs
 * 
 * Defines members specific to RakingTurret child of AlgorithmicTurret:
 * - Fires a variable number of bullets from opposite ends of one diameter
 * - TurretBarrels swivel left and right as the turret rotates
 * - Medium velocity bullets
 * - High rate of fire
 * - No acceleration
 * - Yellow bullets
 */

using UnityEngine;
using System.Collections;

public class RakingTurret : AlgorithmicTurret {
	public int NUM_BARRELS = 4;
	public float TURRET_RADIUS = 4;
	public float MAX_SWIVEL = Mathf.PI/3;
	public float SWIVEL_PERCENTAGE = 0.1f;
	public float BULLET_SEPARATION = 0;
	private float BULLET_WIDTH;
	// Z-component must be a factor of 360
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	private int numFire = 0;
	private bool rotatingLeft = true;

	// Use this for initialization
	void Start () {
		bulletColor = Color.yellow;
		barrelList = new TurretBarrel[NUM_BARRELS];
		Vector3 forwardNorm = transform.forward;
		forwardNorm.Normalize ();
		endOfTurret = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);
		BULLET_WIDTH = (float)bulletPrefab.GetComponent<Renderer> ().bounds.size.x;

		for (int numBarrel = 0; numBarrel < NUM_BARRELS; ++numBarrel) {
			barrelList[numBarrel] = new TurretBarrel (BULLET_WIDTH, 
				(int)bulletVel);
		}
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
	}

	/*
	 * Description: Shoots NUM_BARRELS bullets, half from one side of the turret and half from the other, as the TurretBarrels swivel left and right
	 * Post: A bullet has been fired from all TurretBarrels
	 */
	protected void Fire() {
		Vector3 aimDirNorm = transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();
		Vector3 upNorm = transform.up;
		upNorm.Normalize ();

		//create each bullet
		for (int numBarrel = 0; numBarrel < NUM_BARRELS; ++numBarrel) {
			Quaternion rotQuat = Quaternion.AngleAxis (360f / NUM_BARRELS * numBarrel, transform.forward);
			float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			Vector3 aimRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
			randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			aimRotNorm = Vector3.RotateTowards(aimRotNorm, upNorm, randomRads, 0);
			Vector3 rotVector = Vector3.RotateTowards (aimRotNorm, rightNorm, numFire * MAX_SWIVEL * SWIVEL_PERCENTAGE, 0f);
			if (numBarrel % 2 == 0) {
				CreateBullet (endOfTurret - (rightNorm * TURRET_RADIUS) - (Mathf.Floor (numBarrel / 2) * (BULLET_WIDTH + BULLET_SEPARATION) * rightNorm), rotVector * 2);
			} else {
				CreateBullet (endOfTurret + (rightNorm * TURRET_RADIUS) + (Mathf.Floor (numBarrel / 2) * (BULLET_WIDTH + BULLET_SEPARATION) * rightNorm), rotVector * 2);
			}
		}

		if (Mathf.Abs (numFire) + 1 > 1 / SWIVEL_PERCENTAGE) {
			rotatingLeft = !rotatingLeft;
		}

		if (rotatingLeft)
			--numFire;
		else
			++numFire;

		//rotate the turret by the given angle
		transform.Rotate (ROTATION_ANGLE);
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		InvokeRepeating ("DeathBullet", 0f, 0.05f);
	}

	private void DeathBullet () {
		Vector3 aimDirNorm = transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();
		Vector3 upNorm = transform.up;
		upNorm.Normalize ();

		for (int numBarrel = 0; numBarrel < NUM_BARRELS; ++numBarrel) {
			Quaternion rotQuat = Quaternion.AngleAxis (360f / NUM_BARRELS * numBarrel, transform.forward);
			float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			Vector3 aimRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
			randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
			aimRotNorm = Vector3.RotateTowards(aimRotNorm, upNorm, randomRads, 0);
			Vector3 rotVector = Vector3.RotateTowards (aimRotNorm, rightNorm, numFire * MAX_SWIVEL * SWIVEL_PERCENTAGE, 0f);
			if (numBarrel % 2 == 0) {
				CreateBullet (endOfTurret - (rightNorm * TURRET_RADIUS) - (Mathf.Floor (numBarrel / 2) * (BULLET_WIDTH + BULLET_SEPARATION) * rightNorm), rotVector * 2f);
			} else {
				CreateBullet (endOfTurret + (rightNorm * TURRET_RADIUS) + (Mathf.Floor (numBarrel / 2) * (BULLET_WIDTH + BULLET_SEPARATION) * rightNorm), rotVector * 2f);
			}
		}

		++numDeathBullet;
		if (numDeathBullet == 10) {
			CancelInvoke ("DeathBullet");
		}
	}
}
