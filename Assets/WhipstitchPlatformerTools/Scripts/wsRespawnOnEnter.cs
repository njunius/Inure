using UnityEngine;

public class wsRespawnOnEnter : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		wsPlatformCharacter myCharacter = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			myCharacter.respawn();
		}
	}//	End method OnTriggerEnter2D

	void OnTriggerEnter(Collider other) {
		wsPlatformCharacter myCharacter = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			myCharacter.respawn();
		}
	}//	End method OnTriggerEnter

}//	End class wsRespawnOnEnter
