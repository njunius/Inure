using UnityEngine;
using System.Collections;

public class wsOneWayPlatform : MonoBehaviour {

	private Collider2D myCollider;
	private Collider myCollider3D;

	private void Awake() {
		Collider2D[] colliders = GetComponentsInParent<Collider2D>();
		if (colliders.Length < 2) {
			Collider[] colliders3D = GetComponentsInParent<Collider>();
			if (colliders3D.Length > 1) {
				myCollider3D = colliders3D[1];
			}
		}
		else {
			myCollider = colliders[1];
		}
	}//	End Unity method Awake

	public void allowPassthrough(GameObject other) {
		#if UNITY_3_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3
			if (myCollider != null) {
				myCollider.enabled = false;
			}
			else if (myCollider3D != null) {
				myCollider3D.enabled = false;
			}
		#else
			if (myCollider != null) {
				Collider2D[] allColliders = other.GetComponentsInChildren<Collider2D>();
				foreach (Collider2D thisCollider in allColliders) {
					Physics2D.IgnoreCollision(myCollider, thisCollider, true);
				}
			}
			else if (myCollider3D != null) {
				Collider[] allColliders3D = other.GetComponentsInChildren<Collider>();
				foreach (Collider thisCollider3D in allColliders3D) {
					Physics.IgnoreCollision(myCollider3D, thisCollider3D, true);
				}
			}
		#endif
		wsPlatformCharacter myCharacter = other.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			myCharacter.disableGroundTest();
		}
	}//	End method allowPassthrough

	public void disablePassthrough(Collider2D other) {
		#if UNITY_3_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3
			myCollider.enabled = true;
		#else
			Collider2D[] allColliders = other.GetComponentsInChildren<Collider2D>();
			foreach (Collider2D thisCollider in allColliders) {
				Physics2D.IgnoreCollision(myCollider, thisCollider, false);
			}
		#endif
		wsPlatformCharacter myCharacter = other.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			StartCoroutine(myCharacter.sleepGroundTest(0.1f));
		}
	}//	End method disablePassthrough

	public void disablePassthrough3D(Collider other) {
		#if UNITY_3_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3
			myCollider3D.enabled = true;
		#else
			Collider[] allColliders3D = other.GetComponentsInChildren<Collider>();
			foreach (Collider thisCollider3D in allColliders3D) {
				Physics.IgnoreCollision(myCollider3D, thisCollider3D, false);
			}
		#endif
		wsPlatformCharacter myCharacter = other.GetComponent<wsPlatformCharacter>();
		if (myCharacter != null) {
			StartCoroutine(myCharacter.sleepGroundTest(0.1f));
		}
	}//	End method disablePassthrough3D

	void OnTriggerEnter2D(Collider2D other) {
		allowPassthrough(other.gameObject);
	}//	End method OnTriggerEnter2D

	void OnTriggerStay2D(Collider2D other) {
		allowPassthrough(other.gameObject);
	}//	End method OnTriggerEnter2D

	void OnTriggerExit2D(Collider2D other) {
		disablePassthrough(other);
	}//	End method OnTriggerLeave2D

	void OnTriggerEnter(Collider other) {
		allowPassthrough(other.gameObject);
	}//	End method OnTriggerEnter

	void OnTriggerStay(Collider other) {
		allowPassthrough(other.gameObject);
	}//	End method OnTriggerEnter

	void OnTriggerExit(Collider other) {
		disablePassthrough3D(other);
	}//	End method OnTriggerLeave

}//	End class wsOneWayPlatform
