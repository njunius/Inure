using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnLightsOff : MonoBehaviour {
	public string[] tagsToTurnOff;

	private List<GameObject []> arrayList = new List<GameObject []> ();
	private List<GameObject> lightList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		for(int i = 0; i < tagsToTurnOff.Length; i++){
			arrayList.Add(GameObject.FindGameObjectsWithTag (tagsToTurnOff[i]));
		}

		for(int i = 0; i < arrayList.Count; i++){
			for(int j = 0; j < arrayList[i].Length; j++){
				lightList.Add(arrayList[i][j]);
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player Turret Trigger")) {
			toggleTurrets ();
		}
	}

	void toggleTurrets () {
		for (int numLight = 0; numLight < lightList.Count; ++numLight) {
			lightList [numLight].GetComponent<Light> ().enabled = false;
		}
	}
}
