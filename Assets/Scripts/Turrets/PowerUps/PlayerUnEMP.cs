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
	}
}
