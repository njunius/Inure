using UnityEngine;
using System.Collections;

public class RepoolParticles : MonoBehaviour {

	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem> ();
	}

	// Update is called once per frame
	void Update () {
		if (particles) {
			if (!particles.IsAlive ()) {
				particles.Stop ();
				gameObject.SetActive (false);
			}
		}
	}
}
