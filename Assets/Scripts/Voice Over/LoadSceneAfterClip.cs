﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAfterClip : MonoBehaviour {

    private AudioSource voiceOver;
    private float audioClipStartDelay;
    private float audioClipEndDelay;

    public string sceneToLoad;

    // Use this for initialization
    void Start () {
        voiceOver = gameObject.GetComponent<AudioSource>();
        audioClipStartDelay = 2;
        audioClipEndDelay = 0;

        voiceOver.PlayDelayed(audioClipStartDelay);

        // add start and end delays to the audio clip to make sure the next scene doesn't load too early
        Invoke("loadScene", audioClipStartDelay + voiceOver.clip.length + audioClipEndDelay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void loadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
