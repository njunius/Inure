using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

	private static BulletDestroy destroyer;

	void OnEnable() {
		Debug.Log ("WHY U NO INVOKE");
		Invoke ("Destroy", 2f);
	}

	public void Destroy() {
		gameObject.SetActive (false);
	}

	void OnDisable() {
		CancelInvoke ();
	}
}
