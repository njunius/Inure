using UnityEngine;
using System.Collections;

public class TurretUnEMP : MonoBehaviour {

	private float effectDuration = 5f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (!gameObject.GetComponent<Turret> ().enabled) {
			Invoke ("UnEMP", effectDuration);
		}
	}

	private void UnEMP () {
		gameObject.GetComponent<Turret> ().enabled = true;
	}
}
