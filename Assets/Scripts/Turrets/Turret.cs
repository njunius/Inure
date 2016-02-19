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

	//protected enum Velocity {Low = 1, Medium = 4, High = 7, Extreme = 10, EuropeanExtreme = 30};
	//protected enum RateOfFire {Low = 20, Medium = 10, High = 5, Extreme = 2};
	protected Quaternion zQuat = new Quaternion (0f, 0f, 0f, 0f);

	protected Vector3 turretPos;
	protected Quaternion turretRot = new Quaternion(0, 0, 0, 0);
	//higher number = higher velocity
	public float bulletVel;
	protected Vector4 bulletColor;
	protected TurretBarrel[] barrelList;
	//lower number = higher rate
	public float fireRate;
	protected float fireRateMultiplier = 0.1f;
	protected bool isOn = false;
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
		//LightBulletController bulletObj = (LightBulletController)obj.GetComponent (typeof(LightBulletController));
		Bullet bulletObj = (Bullet)obj.GetComponent (typeof(Bullet));
		bulletObj.setVars (bulletColor, aimDirectionNorm * (float)bulletVel);
		obj.SetActive (true);
	}

	public void TurnOn () {
		isOn = true;
	}

	public void TurnOff () {
		isOn = false;
	}
}
