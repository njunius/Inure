/*
 * TurretBarrel.cs
 * 
 * Defines specifics for individual barrel of a Turret:
 * - spawn point of bullet relative to corresponding Turret
 */

using UnityEngine;
using System.Collections;

public class TurretBarrel {

	public int bulletVel;
	//public Vector3 relativePosition;
	public float relativeSpawnPoint;

	public TurretBarrel (float bWidth, int bVel) {
		relativeSpawnPoint = bWidth;
		bulletVel = bVel;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
