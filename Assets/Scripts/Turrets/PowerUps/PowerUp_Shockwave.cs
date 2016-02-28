﻿using UnityEngine;
using System.Collections;

public class PowerUp_Shockwave : PowerUp {

	private float MAX_FIELD_SIZE = 10f;
	private float FIELD_EXPANSION_PER_FRAME;
	private GameObject effectField;
	private Color fieldColor = new Color (0.988f, 0.541f, 0.541f, 0.294f);
	private bool isGrowing = false;

	// Use this for initialization
	void Start () {
		FIELD_EXPANSION_PER_FRAME = MAX_FIELD_SIZE / 50f;
		effectField= (GameObject)Resources.Load("FieldSphereShockwave");
		effectField = (GameObject)Instantiate (effectField, transform.position, Quaternion.identity);
		effectField.GetComponent<Renderer> ().material.SetColor ("_Color", fieldColor);
		effectField.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (!effectField.activeSelf) {
				effectField.SetActive(true);
				isGrowing = true;
			}
			if (isGrowing) {
				float scaleVal = Mathf.Min (effectField.transform.lossyScale.x + FIELD_EXPANSION_PER_FRAME, MAX_FIELD_SIZE);
				effectField.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
				if (scaleVal == MAX_FIELD_SIZE) {
					isGrowing = false;
					Deactivate ();
				}
			}
		}
	}

	private void Deactivate() {
		effectField.transform.localScale = Vector3.zero;
		effectField.SetActive(false);
		isActive = false;
	}
}
