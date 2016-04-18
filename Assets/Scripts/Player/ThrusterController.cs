using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour {

	public ParticleSystem particleEffect;

	// Use this for initialization
	void Start () {
		TurnOn ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnOn () {
		particleEffect.Play ();
		Invoke ("TurnOff", 5);
	}

	public void TurnOff () {
		particleEffect.Stop ();
		Invoke ("TurnOn", 2);
	}
}
