using UnityEngine;
using System.Collections;

public class T_Turret : SimpleTurret {

	T_Turret() {
		bulletVel = Velocity.Extreme;
		bulletColor = Color.cyan; //FIND HEX FOR TURQUOISE
		fireRate = RateOfFire.High;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
