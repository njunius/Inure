﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Effect_EMP : MonoBehaviour {

	private float effectDuration = 5f;
	private bool isPlayer = false;

    public AudioMixerSnapshot muffled;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			if (!isPlayer && other.gameObject.transform.parent.gameObject.GetComponent<PlayerController> ().enabled) {
				GameObject player = other.gameObject.transform.parent.gameObject;
				player.GetComponent<PlayerController> ().enabled = false;
				player.transform.FindChild ("Main Thruster Left").GetComponent<ParticleSystem> ().Stop ();
				player.transform.FindChild ("Main Thruster Right").GetComponent<ParticleSystem> ().Stop ();
				player.transform.FindChild ("Player EMP Visual").GetComponent<ParticleSystem> ().Play ();
                muffled.TransitionTo(0.5f);
			}
		} else if (other.gameObject.CompareTag ("Turret")) {
			if (!other.gameObject.GetComponent<Turret> ().GetIsEMP ()) {
				other.gameObject.GetComponent<Turret> ().EMP ();
			}
		}
	}

	public void IsPlayer () {
		isPlayer = true;
	}
}
