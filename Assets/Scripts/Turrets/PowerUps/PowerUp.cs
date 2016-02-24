using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	protected bool isActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate() {
		isActive = true;
	}
}
