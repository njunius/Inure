/*
 * SimpleTurret.cs
 * 
 * Defines members specific to SimpleTurret child of Turret:
 * - Only fire toward player and have fairly slow rates of fire
 *   to make up for slightly higher bullet velocities and active
 *   tracking of player
 * - Only fire when player is within sensor range
 * - All bullet-firing patterns are made 3-D by rotation of turret
 *   around local z-axis between shots
 */

using UnityEngine;
using System.Collections;

public class SimpleTurret : Turret {

	public GameObject target;
	protected float sensorRange = 30;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
