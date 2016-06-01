using UnityEngine;
using System.Collections;

public class PlayerUnEMP : MonoBehaviour {

	private float effectDuration = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.GetComponent<PlayerController> ().enabled) {
			Invoke ("UnEMP", effectDuration);
		}
	}

	private void UnEMP () {
		gameObject.GetComponent<PlayerController> ().enabled = true;
		gameObject.transform.FindChild ("Main Thruster Left").GetComponent<ParticleSystem> ().Play ();
		gameObject.transform.FindChild ("Main Thruster Right").GetComponent<ParticleSystem> ().Play ();
		gameObject.transform.FindChild ("Player EMP Visual").GetComponent<ParticleSystem> ().Stop ();
	}
}
