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
		if (hit.gameObject.name == "Player") {
			
		}

		Destroy (gameObject);
	}
}
