using UnityEngine;
using System.Collections;

public class fakeOutTrigger : MonoBehaviour {
	public float minimum = 0;
	public float maximum = -98;
	public float speed = 10;

	public GameObject door;

	private bool slam = false, hasClosed = false;

	private GameObject light;
	private GameObject target;

	// Use this for initialization
	void Start () {
		//door = GameObject.FindGameObjectWithTag("Gate");
		target = gameObject.transform.GetChild(0).gameObject;
		light = door.transform.GetChild(1).gameObject;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player Collider") && !hasClosed) {
			slam = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		if(slam){
			door.transform.position = 
				Vector3.MoveTowards(door.transform.position, target.transform.position, 
					speed * Time.deltaTime);
		}

		if(Vector3.Distance(door.transform.position, target.transform.position) < 1){
			slam = false;
			hasClosed = true;
			light.GetComponent<Light>().enabled = true;
		}
		


	}
}
