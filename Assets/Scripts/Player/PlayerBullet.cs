using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * Description: Sets values for empty vars
	 * post: color and velocity of bullet are set
	 */
	public void setVars (Color bColor, Vector3 newVel) {
		if (bColor != null)
			gameObject.GetComponent<Renderer> ().material.color = bColor;
		if (newVel != null)
			gameObject.GetComponent<Rigidbody> ().velocity = newVel;
	}

	void OnCollisionEnter (Collision hit) {
		if (hit.gameObject.tag != "Player Projectile") {
			ContactPoint contact = hit.contacts [0];
			GameObject particles = (GameObject) Instantiate (Resources.Load ("Particle Systems/Bullet Collision"), contact.point + contact.normal * 2, Quaternion.FromToRotation (Vector3.forward, contact.normal));
			//particles.transform.GetChild (0).gameObject.transform.rotation = particles.transform.rotation;
			particles.GetComponent<ParticleSystem> ().startColor = GetComponent<Renderer> ().material.color;
			particles.transform.GetChild(0).GetComponent<ParticleSystem> ().startColor = GetComponent<Renderer> ().material.color;
			Destroy (gameObject);
		}
	}

	public void SlowTime (float timeScale) {
		gameObject.GetComponent<Rigidbody> ().velocity *= timeScale;
	}

	public void QuickTime (float timeScale) {
		gameObject.GetComponent<Rigidbody> ().velocity /= timeScale;
	}
}
