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
	public float BULLET_FREQUENCY = 7f;
	//multiplied by normalized vector on the local 
	public float BARREL_SEPARATION = 0.25f;
	public Vector3 ROTATION_ANGLE = new Vector3 (0f, 0f, 15f);

	// Use this for initialization
	void Start () {
		bulletColor = Color.magenta;
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

        respawnTurretTick();

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
	 * Description: Creates a certain number of bullets depending on which part of the triangle is to be fired
	 * Pre: numShots is within the set {0, 1, 2}
	 * Post: If numShots is equal to 0, one bullet is fired from the central barrel
	 *       If numShots is equal to 1, one bullet is fired from each barrel directly beside the central barrel
	 *       If numShots is equal to 2, one bullet is fired from each of the end barrels, and numShots is reset
	 */
	protected void SingleBurst() {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();

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
			CancelInvoke ("SingleBurst");
			numShots = 0;

			break;
		default:
			break;
		}
	}

	private void Die () {
		isDead = true;
		isFiring = false;
		isOn = false;
		CancelInvoke ("Fire");
		CancelInvoke ("SingleBurst");
		Explode ();
		InvokeRepeating ("DeathBullet", 0f, 0.05f);
	}

	private void DeathBullet () {
		Vector3 aimDirNorm = transform.forward;
		aimDirNorm.Normalize ();
		Vector3 rightNorm = transform.right;
		rightNorm.Normalize ();
		Vector3 upNorm = transform.up;
		upNorm.Normalize ();

		float randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 topRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		topRotNorm = Vector3.RotateTowards(topRotNorm, upNorm, randomRads, 0);

		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 leftRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		leftRotNorm = Vector3.RotateTowards(leftRotNorm, upNorm, randomRads, 0);

		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 cLeftRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		cLeftRotNorm = Vector3.RotateTowards(cLeftRotNorm, upNorm, randomRads, 0);

		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 cRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		cRotNorm = Vector3.RotateTowards(cRotNorm, upNorm, randomRads, 0);

		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 cRightRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		cRightRotNorm = Vector3.RotateTowards(cRightRotNorm, upNorm, randomRads, 0);

		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		Vector3 rightRotNorm = Vector3.RotateTowards(aimDirNorm, rightNorm, randomRads, 0);
		randomRads = Random.Range (-1 * Mathf.PI / 24, Mathf.PI / 24);
		rightRotNorm = Vector3.RotateTowards(rightRotNorm, upNorm, randomRads, 0);

		CreateBullet (endOfTurret + (2 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [0].relativeSpawnPoint)), leftRotNorm * 2f);
		CreateBullet (endOfTurret + (rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [1].relativeSpawnPoint)), cLeftRotNorm * 2f);
		CreateBullet (endOfTurret + (aimDirNorm * (barrelList [2].relativeSpawnPoint)), cRotNorm * 2f);
		CreateBullet (endOfTurret + (-1 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [3].relativeSpawnPoint)), cRightRotNorm * 2f);
		CreateBullet (endOfTurret + (-2 * rightNorm * BARREL_SEPARATION) + (aimDirNorm * (barrelList [4].relativeSpawnPoint)), rightRotNorm * 2f);

		++numDeathBullet;
		if (numDeathBullet == 10) {
			CancelInvoke ("DeathBullet");
		}
	}
}
