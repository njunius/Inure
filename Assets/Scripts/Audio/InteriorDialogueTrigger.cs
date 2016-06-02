using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteriorDialogueTrigger : MonoBehaviour {
    private AudioSource source;
    private bool hasBeenPlayed = false;

    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot voiced;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (hasBeenPlayed && !source.isPlaying)
        {
            normal.TransitionTo(0.25f);
            this.gameObject.SetActive(false);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenPlayed && other.CompareTag("Player Turret Trigger"))
        {
            source.Play();
            voiced.TransitionTo(0.25f);
            hasBeenPlayed = true;
        }
    }
}
