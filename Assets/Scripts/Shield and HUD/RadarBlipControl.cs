/*
 * RadarBlipControl.cs
 * 
 * Alters the color of blips depending on how far in front of or behind the
 * player bullets are
 */

using UnityEngine;
using System.Collections;

public class RadarBlipControl : MonoBehaviour {

	private Renderer radarRenderer;
	private float colorScale;

	// Use this for initialization
	void Start () {
		radarRenderer = GameObject.FindGameObjectWithTag("Radar3D").GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		colorScale = (transform.localPosition.z + radarRenderer.bounds.extents.x) / radarRenderer.bounds.size.x;
		transform.LookAt (GameObject.FindGameObjectWithTag ("MainCamera").transform);
		float redVal = ((colorScale < 0.5) ? 1f : 2f);
		//if the bullet is between the player and the back of the warning radius
		if (colorScale <= 0.5) {
			//scale G and B values of the color, scaling color between white and red
			gameObject.GetComponent<Renderer> ().material.color = new Color (1f, 1f - 2 * colorScale, 1f - 2 * colorScale);
		} else {
			//scale R value of the color, scaling color between red and black
			colorScale = transform.localPosition.z / radarRenderer.bounds.extents.x;
			gameObject.GetComponent<Renderer> ().material.color = new Color (1f - colorScale, 0f, 0f);
		}
	}
}
