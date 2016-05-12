using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	protected bool isOn = false;
	protected bool isEMP = false;
	protected bool isFiring = false;
	protected bool isSlowed = false;
	protected float health = 100;
	public float healthDecrement = 10f;
	protected bool isDead = false;
	public float fireRate;
	protected float fireDelay = 0f;
	private float effectDuration = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOn () {
		if(!isDead)
			isOn = true;
	}

	public void TurnOff () {
		isOn = false;
	}

	public bool GetIsEMP () {
		return isEMP;
	}

	public void EMP () {
		isEMP = true;
		Invoke ("UnEMP", effectDuration);
	}

	private void UnEMP () {
		isEMP = false;
	}

	private void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.name == "PlayerBullet(Clone)") {
			takeDamage (healthDecrement);
		}
	}

	public void takeDamage(float damage) {
		health = Mathf.Max (health - damage, 0f);
		GameObject newSparks = (GameObject)Instantiate (Resources.Load ("Particle Systems/Flare"), transform.position, Quaternion.identity);//collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point - transform.position));
		//newSparks.transform.parent = gameObject.transform;
		//newSparks.GetComponent<ParticleSystem> ().Play ();
		newSparks.SetActive(true);
		StartCoroutine (TurnOffSparks (newSparks, 3f));
	}

	private IEnumerator TurnOffSparks (GameObject particles, float delayTime) {

		yield return new WaitForSeconds (delayTime);
		particles.GetComponent<ParticleSystem> ().Stop ();
		//particles.SetActive (false);
		yield return new WaitForSeconds (delayTime);
		Destroy (particles);
	}

	protected void Explode() {
		GameObject newSparks = (GameObject)Instantiate (Resources.Load ("Particle Systems/TurretExplosion"), transform.position, Quaternion.identity);
	}
}
