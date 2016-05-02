using UnityEngine;
using System.Collections;

public class DestroyContrail : MonoBehaviour {

	private bool wasGivenParent = false;
	private bool isTriggered = false;
	private float time;

	// Use this for initialization
	void Start () {
		time = GetComponent<TrailRenderer> ().time;
	}
	
	// Update is called once per frame
	void Update () {
		if (wasGivenParent && !isTriggered && transform.parent == null) {
			isTriggered = true;
		} else if (isTriggered) {
			Die ();
		}
	}

	public void GiveParent (Transform parent) {
		transform.parent = parent;
		wasGivenParent = true;
	}

	private void Die () {
		transform.parent = transform;

		Destroy (gameObject, time);
	}
}
