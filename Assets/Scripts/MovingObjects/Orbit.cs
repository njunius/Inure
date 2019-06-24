using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	public float rotation_rate;
	public GameObject orbit_point;
    public Rigidbody rb;
    public Quaternion rotationAngle;
	void fixedUpdate()
	{
		//gameObject.transform.Rotate(rotation_rate * Time.deltaTime);
	}

	// Use this for initialization
	void Start ()
	{
        rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.RotateAround(orbit_point.transform.position, Vector3.up, rotation_rate * Time.deltaTime);

        //rotationAngle = Quaternion.AngleAxis(rotation_rate * Time.deltaTime, Vector3.up);
        //rb.MovePosition(rotationAngle * (rb.transform.position - orbit_point.transform.position) + orbit_point.transform.position);
        //rb.MoveRotation(rb.transform.rotation * rotationAngle);
	}

	public float GetRotationRate () {
		return rotation_rate;
	}

	public void SetRotationRate (float newRate) {
		rotation_rate = newRate;
	}
}
