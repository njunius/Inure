using UnityEngine;
using System.Collections;

public class PowerBombExplosion : MonoBehaviour {

	private float explosionSize;
	private bool isExploding = false;
	private bool damagedPlayer = false;
	private float alphaIncrement;
	private float time = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isExploding && transform.localScale.x < explosionSize) {
			Color currColor = GetComponent<Renderer> ().material.color;
			GetComponent<Renderer> ().material.color = new Color(currColor.r, currColor.g, currColor.b, currColor.a - alphaIncrement);
			Vector3 currScale = transform.localScale;
			float newScale = Mathf.Min (currScale.x + (100f * Time.deltaTime), explosionSize);
			transform.localScale = new Vector3 (newScale, newScale, newScale);
			time += Time.deltaTime;
		} else if (transform.localScale.x != 0) {
			Destroy (gameObject);
		}
	}

	public void Explode (float size) {
		explosionSize = size;
		isExploding = true;
		alphaIncrement = Time.deltaTime / (explosionSize / 100f);
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Turret")) {
			other.gameObject.GetComponent<Turret> ().takeDamage (transform.parent.GetComponent<PowerBomb> ().GetPowerLevel () * 2);
		} else if (other.CompareTag ("Player Collider")) {
			GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<PlayerController> ().takeDamage ();
		}
	}
}
