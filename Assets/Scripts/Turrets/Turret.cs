/*
 * Turret.cs
 * 
 * Creates or defines specifics common among all children of Turret:
 * - Velocity and rate-of-fire keys
 * - Orientation of object
 * - Velocity, color, rate-of-fire, and despawn distance of bullets
 * - List of barrels for turret
 */

using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	//higher number = higher velocity
	protected enum Velocity {Low = 10, Medium = 10, High = 10, Extreme = 20};
	//lower number = higher rate
	protected enum RateOfFire {Low = 20, Medium = 10, High = 2, Extreme = 1};
	protected Quaternion zQuat = new Quaternion (0f, 0f, 0f, 0f);

	protected Vector3 turretPos;
	protected Quaternion turretRot = new Quaternion(0, 0, 0, 0);
	protected Velocity bulletVel;
	protected Vector4 bulletColor;
	protected TurretBarrel[] barrelList;
	protected RateOfFire fireRate;
	protected float fireRateMultiplier = 0.1f;
	protected bool isFiring = false;
	protected float fireDelay = 0;
	protected Vector3 endOfTurret;

	protected float despawnDist; //UNDEFINED

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
		turretPos = gameObject.GetComponent<Rigidbody>().position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void CreateBullet (Vector3 position, Vector3 aimDirectionNorm) {
		GameObject obj = ObjectPooler.current.GetPooledObject ();
		if (obj == null)
			return;

		obj.transform.position = position;
		obj.transform.rotation = transform.rotation;
		Bullet bulletObj = (Bullet)obj.GetComponent (typeof(Bullet));
		bulletObj.setVars (bulletColor, aimDirectionNorm * (float)bulletVel);
		obj.SetActive (true);
	}
}
