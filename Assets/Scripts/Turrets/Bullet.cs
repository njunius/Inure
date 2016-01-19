/*
 * Bullet.cs
 * 
 * Defines functionality of individual bullet object:
 * - setting of color and velocity on instantiation
 * - reaction to collision with other GameObjects
 */

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Vector3 velocity;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity * Time.deltaTime;
		//gameObject.GetComponent<Rigidbody> ().velocity = velocity;
		//Debug.Log ("update");
	}

	/*
	 * Description: Sets values for empty vars
	 * post: color and velocity of bullet are set
	 */
	public void setVars (Color bColor, Vector3 newVel) {
		gameObject.GetComponent<Renderer> ().material.color = bColor;
		velocity = newVel;
	}

	void OnCollisionEnter (Collision hit) {
		if (hit.gameObject.tag == "Player" && !hit.gameObject.GetComponent<PlayerController>().isShielded()) {
            // note that the 50 is a placeholder for real damage values later
            // and that the player's health is base 100 for future reference
            hit.gameObject.GetComponent<PlayerController>().takeDamage(50);
		}
		if (hit.gameObject.tag != "Projectile") {
			Destroy (gameObject);
		}
	}
}
