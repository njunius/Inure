using UnityEngine;
using System.Collections;

public class PowerUp_SlowTime : PowerUp {

	private float MAX_FIELD_SIZE = 30f;
	private float FIELD_EXPANSION_PER_FRAME;
	private float LIFETIME = 5;
	private float FIELD_COMPRESSION_PER_FRAME = 0.1f;
	private GameObject effectField;
	private Color fieldColor = new Color (0.988f, 0.541f, 0.992f, 0.294f);
	private bool isGrowing = false;
	private bool isShrinking = false;

	// Use this for initialization
	void Start () {
		FIELD_EXPANSION_PER_FRAME = MAX_FIELD_SIZE / 50f;
		effectField= (GameObject)Resources.Load("FieldSphereSlowTime");
		effectField = (GameObject)Instantiate (effectField, transform.position, Quaternion.identity);
		effectField.GetComponent<Renderer> ().material.SetColor ("_Color", fieldColor);
		effectField.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (!effectField.activeSelf) {
				effectField.SetActive (true);
				isGrowing = true;
			}
			if (isGrowing) {
				float scaleVal = Mathf.Min (effectField.transform.lossyScale.x + FIELD_EXPANSION_PER_FRAME, MAX_FIELD_SIZE);
				effectField.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
				if (scaleVal == MAX_FIELD_SIZE) {
					isGrowing = false;
					Invoke ("Shrink", LIFETIME);
					isShrinking = true;
				}
			} else if (isShrinking) {
				float scaleVal = Mathf.Max (effectField.transform.lossyScale.x - FIELD_COMPRESSION_PER_FRAME, 0f);
				effectField.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
				if (scaleVal == 0f) {
					isShrinking = false;
					Deactivate ();
				}
			}
		}
	}

	private void Shrink() {
		isShrinking = true;
	}

	private void Deactivate() {
		effectField.transform.localScale = Vector3.zero;
		effectField.SetActive(false);
		isActive = false;
	}
}
