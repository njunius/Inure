using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {

    AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!source.isPlaying)
        {
            source.Play();
        }
	}
}
