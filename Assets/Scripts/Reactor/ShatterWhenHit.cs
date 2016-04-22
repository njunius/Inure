using UnityEngine;
using System.Collections;

public class ShatterWhenHit : MonoBehaviour {
	public GameObject shatterInto;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//when the player shoots the thing, create the child object to shatter
	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "Player Projectile"){
			Instantiate(shatterInto, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
