using UnityEngine;
using System.Collections;

public class ScaleEMPField : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//GameObject child = transform.FindChild ("EMP Particles").gameObject;
		for (int numSystem = 0; numSystem < transform.childCount - 1; numSystem++) {
			Transform particles = transform.GetChild (numSystem);
			particles.localScale = transform.localScale;
		}
		transform.GetChild (3).localScale = transform.localScale / 20;
	}
}
