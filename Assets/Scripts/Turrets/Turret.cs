using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	protected bool isOn = false;
	protected bool isFiring = false;
	public float fireRate;
	protected float fireDelay = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOn () {
		isOn = true;
	}

	public void TurnOff () {
		isOn = false;
	}
}
