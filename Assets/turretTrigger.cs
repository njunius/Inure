﻿using UnityEngine;
using System.Collections;

public class turretTrigger : MonoBehaviour {

	public GameObject[] turretsInRoom = new GameObject[2];

	// First, turn off turrets
	void Start () {
		for (int i = 0; i < turretsInRoom.Length; i++) {
			turretsInRoom [i].SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player Collider")){
			//Debug.Log ("Player is in");
			for (int i = 0; i < turretsInRoom.Length; i++) {
				//Debug.Log ("Activating Turret");
				turretsInRoom [i].SetActive(true);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
