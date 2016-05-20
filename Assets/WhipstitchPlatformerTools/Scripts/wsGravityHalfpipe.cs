using UnityEngine;
using System.Collections;

public class wsGravityHalfpipe : MonoBehaviour {

	void OnTriggerStay2D(Collider2D coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.setGravityToGroundPlane();
	}//	End method OnTriggerStay2D

	void OnTriggerStay(Collider coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.setGravityToGroundPlane();
	}//	End method OnTriggerStay

	void OnTriggerExit2D(Collider2D coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.setGravityToGroundPlane();
	}//	End method OnTriggerExit2D

	void OnTriggerExit(Collider coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.setGravityToGroundPlane();
	}//	End method OnTriggerExit

}//	End class wsGravityHalfpipe
