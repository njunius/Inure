﻿using UnityEngine;
using System.Collections;

public class RadarTrigger : MonoBehaviour {

	private static Radar3D radar;
	private static float radius;

	void OnEnable () {
		radar = GameObject.FindGameObjectWithTag ("Radar3D").GetComponent<Radar3D> ();
		radius = gameObject.GetComponent<SphereCollider> ().radius;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Projectile")) {
			radar.AddBlip (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Projectile")) {
			radar.RemoveBlip (other.gameObject);
			other.GetComponent<Bullet> ().brackets.enabled = false;
		}
	}
}