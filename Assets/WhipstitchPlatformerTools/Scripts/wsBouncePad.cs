using UnityEngine;
using System.Collections;

public class wsBouncePad : MonoBehaviour {

	[SerializeField] private float bounceForce;
	[SerializeField] private bool bounceOnImpactsOnly;
	
	void OnTriggerEnter2D(Collider2D other) {
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		if (!bounceOnImpactsOnly || (bounceOnImpactsOnly && other.attachedRigidbody.velocity.y < 0.0f)) {
			character.bounce(bounceForce);
		}
	}//	End method OnTriggerEnter2D
	
	void OnTriggerEnter(Collider other) {
		wsPlatformCharacter character = other.gameObject.GetComponent<wsPlatformCharacter>();
		if (character == null) { return; }
		if (!bounceOnImpactsOnly || (bounceOnImpactsOnly && other.attachedRigidbody.velocity.y < 0.0f)) {
			character.bounce(bounceForce);
		}
	}//	End method OnTriggerEnter

}//	End class wsBouncePad
