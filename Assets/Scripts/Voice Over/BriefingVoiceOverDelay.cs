using UnityEngine;
using System.Collections;

public class BriefingVoiceOverDelay : MonoBehaviour {

    private AudioSource briefingVO;

	// Use this for initialization
	void Awake () {
        briefingVO = gameObject.GetComponent<AudioSource>();

        briefingVO.PlayDelayed(1.5f);
	}

    // Update is called once per frame
    void Update() {

	}
}
