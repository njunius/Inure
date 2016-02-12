/*
 * Bullet.cs
 * 
 * Defines functionality of individual bullet object:
 * - setting of color and velocity on instantiation
 * - reaction to collision with other GameObjects
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightBulletController : MonoBehaviour {

	//private Vector3 velocity;

	private int absorbValue;

	public Image brackets;

	// Use this for initialization
	void Awake () {
		brackets = GetComponentInChildren<Image>();
		absorbValue = 1;
	}

	// Update is called once per frame
	void Update () {
	}

	/*
	 * Description: Sets values for empty vars
	 * post: color and velocity of bullet are set
	 */
	public void setVars (Color bColor, Vector3 newVel) {
		gameObject.GetComponent<Light> ().color = bColor;
		gameObject.GetComponent<Rigidbody> ().velocity = newVel;

	}

	void OnCollisionEnter (Collision hit) {
		PlayerController hitScript = hit.gameObject.GetComponent<PlayerController>();
		if (hit.gameObject.CompareTag("Player") && !hitScript.isShielded()) {
			hitScript.takeDamage();
		}
		//Destroy (gameObject);
		Destroy();
		//gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider volume)
	{
		if (volume.gameObject.CompareTag("Warning Radius"))
		{
			brackets.enabled = true;
		}

	}

	void OnTriggerExit(Collider volume)
	{
		if (volume.gameObject.CompareTag("Warning Radius"))
		{
			brackets.enabled = false;
		}
	}

	public int getAbsorbValue()
	{
		return absorbValue;
	}

	void OnEnable() {
		Invoke ("Destroy", 20f);
	}

	public void Destroy() {
		brackets.enabled = false;
		GameObject radar = GameObject.FindGameObjectWithTag ("Radar3D");
		if (radar) {
			radar.GetComponent<Radar3D> ().RemoveBlip (gameObject);
		}
		GameObject[] threatQuadrants = GameObject.FindGameObjectsWithTag("Threat Quadrant");
		for(int i = 0; i < threatQuadrants.Length; ++i)
		{
			threatQuadrants[i].GetComponent<ThreatTriggerController>().removeListElement(gameObject);
		}
		gameObject.SetActive (false);
	}

	void OnDisable() {
		CancelInvoke ();
	}
}