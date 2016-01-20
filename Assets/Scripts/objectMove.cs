using UnityEngine;
using System.Collections;

public class objectMove : MonoBehaviour {
	public GameObject node1, node2;
	public float speed = 1;
	private float step = 0;
	public Vector3 startPOS, endPOS;

	// Use this for initialization
	void Start () {
		startPOS = node1.transform.position;
		endPOS = node2.transform.position;
	}
	
	//Move the cube towards either the Start or End nodes
	void FixedUpdate () {
		transform.position = Vector3.MoveTowards(transform.position, node2.transform.position, speed);
	}
}
