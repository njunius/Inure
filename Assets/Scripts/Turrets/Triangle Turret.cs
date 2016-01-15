using UnityEngine;
using System.Collections;

public class TriangleTurret : SimpleTurret {

	TriangleTurret() {
		bulletVel = Velocity.Low;
		bulletColor = Color.magenta;
		fireRate = RateOfFire.Medium;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
