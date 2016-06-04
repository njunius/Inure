using UnityEngine;
using System.Collections;

public class PowerUp_Shockwave : PowerUp {

	private float MAX_FIELD_SIZE = 120f;
	private float FIELD_EXPANSION_PER_FRAME;
	private GameObject effectField;
	static private int numFields = 5;
	private GameObject[] fields = new GameObject[numFields];
	private float[] fieldTimes = new float[numFields];
	private bool[] hasSpawned = new bool[numFields];
	private Color fieldColor = new Color (0.541f, 0.541f, 0.988f, 0.5f);
	private bool isGrowing = false;

    public AudioClip shockwaveSound;
    private AudioSource source;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
		FIELD_EXPANSION_PER_FRAME = MAX_FIELD_SIZE / 100f;
		effectField= (GameObject)Resources.Load("FieldSphereShockwave");
		for (int numField = 0; numField < numFields; ++numField) {
			fields[numField] = (GameObject)Instantiate (effectField, transform.position, Quaternion.identity);
			fields[numField].GetComponent<Renderer> ().material.SetColor ("_Color", fieldColor);
			fields[numField].SetActive (false);
			fieldTimes [numField] = MAX_FIELD_SIZE * ((float)numField / (float)numFields);
		}
		//effectField = (GameObject)Instantiate (effectField, transform.position, Quaternion.identity);
		if (gameObject.CompareTag ("Player")) { 																	//GET RID OF THIS
			effectField.GetComponent<Effect_Shockwave> ().IsPlayer ();
		}
		//effectField.GetComponent<Renderer> ().material.SetColor ("_Color", fieldColor);
		//effectField.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (!fields[0].activeSelf) {
				hasSpawned [0] = true;
				for (int numField = 0; numField < numFields; ++numField) {
					fields[numField].SetActive(true);
				}
                source.PlayOneShot(shockwaveSound, 0.6f);
                isGrowing = true;
			}
			if (isGrowing) {
				for (int numField = 0; numField < numFields; ++numField) {
					if (numField == 0) {
						float scaleVal = Mathf.Min (fields[0].transform.lossyScale.x + FIELD_EXPANSION_PER_FRAME, MAX_FIELD_SIZE); //lossy
						fields[0].transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
						if (scaleVal == MAX_FIELD_SIZE) {
							isGrowing = false;
							for (int numField2 = 0; numField2 < numFields; ++numField2) {
								hasSpawned [numField2] = false;
							}
							Deactivate ();
							break;
						}
					}else if (hasSpawned[numField]) {
						float scaleVal = Mathf.Min (fields [numField].transform.lossyScale.x + (FIELD_EXPANSION_PER_FRAME / (1 - ((float)numField / (float)numFields))), MAX_FIELD_SIZE); // lossy
						fields [numField].transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
					}else if (fields[0].transform.lossyScale.x >= fieldTimes[numField]) { // lossy
						hasSpawned[numField] = true;
						break;
					}
				}

				//float scaleVal = Mathf.Min (effectField.transform.lossyScale.x + FIELD_EXPANSION_PER_FRAME, MAX_FIELD_SIZE);
				//effectField.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
			}
		}
	}

	private void Deactivate() {
		for (int numField = 0; numField < numFields; ++numField) {
			fields[numField].transform.localScale = Vector3.zero;
			fields[numField].SetActive(false);
			isActive = false;
		}
		//effectField.transform.localScale = Vector3.zero;
		//effectField.SetActive(false);
		//isActive = false;
	}
}