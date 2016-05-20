using UnityEngine;
using System.Collections;

public class wsWallJumpTrigger : MonoBehaviour {

	[SerializeField] private bool leftFacingWall;

	void OnTriggerEnter2D(Collider2D other) {
		if (Mathf.Abs(Vector3.Dot(other.transform.right, transform.right)) != 1.0f) { return; }
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		character.wallSlide(this);
	}//	End method OnTriggerEnter2D

	void OnTriggerExit2D(Collider2D other) {
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		character.detachFromWall(this);
	}//	End method OnTriggerEnter2D

	void OnTriggerEnter(Collider other) {
		if (Mathf.Abs(Vector3.Dot(other.transform.right, transform.right)) != 1.0f) { return; }
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		character.wallSlide(this);
	}//	End method OnTriggerEnter

	void OnTriggerExit(Collider other) {
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		character.detachFromWall(this);
	}//	End method OnTriggerEnter

	public float getDirection() { return leftFacingWall ? -1.0f : 1.0f; }

}//	End class wsWallJumpTrigger
