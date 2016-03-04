using UnityEngine;
using System.Collections;

public class fakeOutTrigger : MonoBehaviour {
	public float minimum = 0;
	public float maximum = -98;
	public float speed = 10;

	private GameObject door;
	private bool slam = false;

	// Use this for initialization
	void Start () {
		door = GameObject.FindGameObjectWithTag("Gate");
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player Collider")) {
			slam = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(slam && door.transform.position.z < maximum){
			door.transform.Translate(0, 0, speed * Time.deltaTime);
		}
	}
}
