/*
 * SpriteBullet.cs
 * 
 * Causes 2D bullet sprites to always face the camera
 */

using UnityEngine;
using System.Collections;

public class SpriteBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(GameObject.FindWithTag("Camera Target").transform);
	}
}
