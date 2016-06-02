using UnityEngine;
using System.Collections;

public class BulletSoundInitializer : MonoBehaviour {
    AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.9f, 1.1f);
	}
	
}
