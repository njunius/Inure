using UnityEngine;
using System.Collections;

public class rotate_blade : MonoBehaviour {
    public Vector3 rotation_rate;
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
        gameObject.transform.Rotate(rotation_rate * Time.deltaTime);
    }
}
