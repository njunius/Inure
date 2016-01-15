using UnityEngine;
using System.Collections;

public class ArcTurret : SimpleTurret {

	ArcTurret() {
		bulletVel = Velocity.High;
		bulletColor = Color.green;
		fireRate = RateOfFire.Medium;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
