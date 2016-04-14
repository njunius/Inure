using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	public float rotation_rate;
	public GameObject orbit_point;
	void fixedUpdate()
	{
		//gameObject.transform.Rotate(rotation_rate * Time.deltaTime);
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		//gameObject.transform.RotateAround(orbit_point.transform.position, Vector3.up, rotation_rate * Time.deltaTime);
	}
}
