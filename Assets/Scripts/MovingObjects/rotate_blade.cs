using UnityEngine;
using System.Collections;

public class rotate_blade : MonoBehaviour {
    public Vector3 rotationVelocity;
	//public float speed = 1;

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
		Quaternion deltaRotation = Quaternion.Euler(rotationVelocity * Time.deltaTime);
		GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * deltaRotation);
    }
}
