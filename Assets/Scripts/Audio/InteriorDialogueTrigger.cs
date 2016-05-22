using UnityEngine;
using System.Collections;

public class InteriorDialogueTrigger : MonoBehaviour {
    public AudioClip audioClip;
    private AudioSource source;
    private bool hasBeenPlayed = false;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenPlayed && other.CompareTag("Player Turret Trigger"))
        {
            source.Play();
            hasBeenPlayed = true;
        }
    }
}
