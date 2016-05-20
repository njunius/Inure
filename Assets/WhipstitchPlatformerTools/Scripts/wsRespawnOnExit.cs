using UnityEngine;

public class wsRespawnOnExit : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other) {
		wsPlatformCharacter myCharacter = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			myCharacter.respawn();
		}
	}//	End method OnTriggerExit2D

	void OnTriggerExit(Collider other) {
		wsPlatformCharacter myCharacter = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			myCharacter.respawn();
		}
	}//	End method OnTriggerExit2D

}//	End class RespanOnExit
