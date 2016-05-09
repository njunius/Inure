using UnityEngine;
using System.Collections;

public class DriftMotion : MonoBehaviour {

    public float minForce = 0.0f;
    public float maxForce = 0.00005f;
    public float minSpin = 0.0f;
    public float maxSpin = 100.0f;
    public Vector3 generalDirection = new Vector3(0, 0, 0);
    public Vector3 spinDirection = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        float force = Random.Range(minForce, maxForce);
        float spin = Random.Range(minSpin, maxSpin);
        if (generalDirection.Equals(Vector3.zero))
        {
            generalDirection = new Vector3(Random.Range(-255, 255), Random.Range(-255, 255), Random.Range(-255, 255));
        }
        generalDirection = force * generalDirection.normalized;
        rb.AddForce(generalDirection);

        if (spinDirection.Equals(Vector3.zero))
        {
            spinDirection = new Vector3(Random.Range(-255, 255), Random.Range(-255, 255), Random.Range(-255, 255));
            
        }
        else
        {
            spinDirection = transform.TransformDirection(spinDirection);
        }
        spinDirection = spin * spinDirection.normalized;
        //Debug.Log(transform.name);
        //Debug.Log(spinDirection);
        rb.AddTorque(spinDirection);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
