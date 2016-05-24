using UnityEngine;
using System.Collections;

public class objectMove : MonoBehaviour {
	//public static int numElements = 2;
	public GameObject[] nodes = new GameObject[2];
	public float speed = 1;
	private float step = 0;
	private int index = 1;
	private bool isMovingTo2 = false;
	private GameObject startPOS, endPOS;

	// Use this for initialization
	void Start () {
		startPOS = nodes[0];
		endPOS = nodes [1];
	}
	
	//Move the cube towards either the Start or End nodes
	void FixedUpdate () {

		if (closeEnough (gameObject, nodes [index]) && index + 1 < nodes.Length) {
			index++;
		} else if (closeEnough (gameObject, nodes [index]) && index + 1 >= nodes.Length) {
			index = 0;
		}
		GetComponent<Rigidbody>().position = Vector3.MoveTowards (transform.position, nodes[index].transform.position, speed);
	}

	bool closeEnough (GameObject point1, GameObject point2){
		return 	Vector3.Distance (point1.transform.position, point2.transform.position) < 0.001;
	}
}
