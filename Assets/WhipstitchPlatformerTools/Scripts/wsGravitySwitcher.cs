//	wsGravitySwitcher.cs
//	D. Scott Nettleton
//	July 24, 2015

using UnityEngine;

public class wsGravitySwitcher : MonoBehaviour {

	private Vector3 gravityDirection;

	private void Start() {
		gravityDirection = -transform.up;
	}//	End Unity method Start

	private void OnTriggerEnter2D(Collider2D other) {
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		character.setGravityDirection(gravityDirection);
	}//	End Unity method OnTriggerEnter2D

	private void OnTriggerEnter(Collider other) {
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		character.setGravityDirection(gravityDirection);
	}//	End Unity method OnTriggerEnter

}//	End class wsGravitySwitcher
