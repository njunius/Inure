/*
 * AlgorithmicTurret.cs
 * 
 * Defines members specific to AlgorithmicTurret child of Turret:
 * - Instead of firing explicitly toward the player, these turrets fire a
 *   rapid stream of slow-moving bullets in a repeating pattern
 * - As these turrets do not track the player, they will continuously fire
 *   regardless of the player being within their fields of fire
 */

using UnityEngine;
using System.Collections;

public class AlgorithmicTurret : Turret {

	public Vector3 focusPoint;
	public GameObject target;
	public float sensorRange = 30f;
	public bool fireOnlyWhenPlayerNear = true;

	// Use this for initialization
	void Start () {
		focusPoint = transform.position + transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (focusPoint);
	}
}
