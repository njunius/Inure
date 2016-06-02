using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerUnEMP : MonoBehaviour {
    public AudioMixerSnapshot voice;
    public AudioMixerSnapshot normal;
    private float effectDuration = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.GetComponent<PlayerController> ().enabled) {
			Invoke ("UnEMP", effectDuration);
		}
	}

	private void UnEMP () {
        
		gameObject.GetComponent<PlayerController> ().enabled = true;
        
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioManager>().voiceTrackPlaying)
        {
            voice.TransitionTo(0.5f);
        }
        else
        {
            normal.TransitionTo(0.5f);
        }
		gameObject.transform.FindChild ("Main Thruster Left").GetComponent<ParticleSystem> ().Play ();
		gameObject.transform.FindChild ("Main Thruster Right").GetComponent<ParticleSystem> ().Play ();
		gameObject.transform.FindChild ("Player EMP Visual").GetComponent<ParticleSystem> ().Stop ();
	}
}
