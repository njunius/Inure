﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 20.0f;
    public float rotSpeed = 180.0f;
    public float rollSpeed = 100.0f;
    public Camera camera;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        

        if (Input.GetAxis("Quit") > 0)
        {
            Application.Quit();
        }
	}

    void FixedUpdate()
    {
        float moveLongitudinal = Input.GetAxis("Longitudinal") * moveSpeed * Time.deltaTime;
        float moveLateral = Input.GetAxis("Lateral") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotPitch = Input.GetAxis("Pitch") * rotSpeed * Time.deltaTime;
        float rotYaw = Input.GetAxis("Yaw") * rotSpeed * Time.deltaTime;
        float rotRoll = Input.GetAxis("Roll") * rollSpeed * Time.deltaTime;


        if (moveLongitudinal != 0 || moveLateral != 0 || moveVertical != 0)
        {
            //rb.velocity += transform.forward * moveLongitudinal;
            //+ transform.right * moveLateral + transform.up * moveVertical;
            rb.transform.Translate(Vector3.forward * moveLongitudinal);
            rb.transform.Translate(Vector3.right * moveLateral);
            rb.transform.Translate(Vector3.up * moveVertical);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
        rb.transform.Rotate(Vector3.left * rotPitch);
        rb.transform.Rotate(Vector3.up * rotYaw);
        rb.transform.Rotate(Vector3.forward * rotRoll);

        if (camera != null)
        {
            camera.transform.rotation = transform.rotation;
            camera.transform.position = transform.position;
            camera.transform.Translate(Vector3.back * 3);
        }
    }
}
