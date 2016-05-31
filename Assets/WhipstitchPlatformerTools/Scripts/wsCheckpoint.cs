using UnityEngine;
using System.Collections;

public class wsCheckpoint : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		wsPlatformCharacter myCharacter = other.gameObject.GetComponent<wsPlatformCharacter>();
		activate(myCharacter);
	}//	End Unity method OnTriggerEnter2D

	void OnTriggerEnter(Collider other) {
		wsPlatformCharacter myCharacter = other.gameObject.GetComponent<wsPlatformCharacter>();
		activate(myCharacter);
	}//	End Unity method OnTriggerEnter2D

	public void activate(wsPlatformCharacter myCharacter) {
		if (myCharacter != null) {
			myCharacter.setRespawnPosition(transform.position);
			deactivate();
		}
	}//	End method activate

	public void deactivate() {
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		if (collider != null) {
			collider.enabled = false;
		}
		else {
			BoxCollider collider3D = GetComponent<BoxCollider>();
			if (collider3D != null) {
				collider3D.enabled = false;
			}
		}
	}//	End method deactivate

}//	End class wsCheckpoint
