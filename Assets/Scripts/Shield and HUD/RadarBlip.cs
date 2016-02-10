/*
 * RadarBlip.cs
 * 
 * Encapsultes members of a single RadarBlip object: the original object being tracked and the blip object itself
 */

using UnityEngine;
using System.Collections;

public class RadarBlip {
	private GameObject orig;
	private GameObject blip;
	private int listPosition;

	public RadarBlip(GameObject obj, GameObject blipObj) {
		orig = obj;
		blip = blipObj;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject GetOrig () {
		return orig;
	}

	public GameObject GetBlip () {
		return blip;
	}

	public bool isEqualTo (GameObject other) {
		return this.orig == other;
	}
}
