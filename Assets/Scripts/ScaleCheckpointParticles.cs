using UnityEngine;
using System.Collections;

public class ScaleCheckpointParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ParticleSystem> ().startLifetime *= transform.parent.localScale.z;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
