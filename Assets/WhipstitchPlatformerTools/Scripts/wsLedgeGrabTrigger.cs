using UnityEngine;
using System.Collections;

public class wsLedgeGrabTrigger : MonoBehaviour {

	[SerializeField] private bool leftSide;
	[SerializeField] private Transform ledgePosition;
	[SerializeField] private Transform ledgePullUpPosition;

	[SerializeField] private wsPlatformCharacter currentCharacter = null;

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.transform.rotation != transform.rotation) { return; }
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		if (currentCharacter != character) {
			if (currentCharacter != null) { currentCharacter.dropFromLedge(); }
			if (character.hasAbility(Abilities.LedgeGrab) && !character.isPullingFromLedge()) {
				character.grabLedge(ledgePosition, leftSide, ledgePullUpPosition);
				currentCharacter = character;
			}
		}
	}//	End method OnTriggerEnter2D

	void OnTriggerExit2D(Collider2D coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == currentCharacter) {
			currentCharacter = null;
		}
	}//	End method OnTriggerExit2D

	void OnTriggerEnter(Collider coll) {
		if (coll.transform.rotation != transform.rotation) { return; }
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		if (currentCharacter != character) {
			if (currentCharacter != null) { currentCharacter.dropFromLedge(); }
			if (character.hasAbility(Abilities.LedgeGrab) && !character.isPullingFromLedge()) {
				character.grabLedge(ledgePosition, leftSide, ledgePullUpPosition);
				currentCharacter = character;
			}
		}
	}//	End method OnTriggerEnter

	void OnTriggerExit(Collider coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == currentCharacter) {
			currentCharacter = null;
		}
	}//	End method OnTriggerExit

}//	End class wsLedgeGrabTrigger
