using UnityEngine;
using System.Collections;

public class BulletBlocker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().material.color = new Color(0f, 0f, 0.1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		//Debug.Log (other.gameObject.CompareTag ("Player Projectile"));
		if (other.gameObject.CompareTag ("Player Projectile") || other.gameObject.CompareTag ("Projectile")) {
			CancelInvoke ("ResetMatColor");
			gameObject.GetComponent<Renderer> ().material.color = Color.blue;
			Invoke ("ResetMatColor", 1f);
		}
	}

	private void ResetMatColor() {
		//gameObject.GetComponent<Renderer> ().material.color = new Color(0f, 0f, 0.1f, 0f);
	}
}
