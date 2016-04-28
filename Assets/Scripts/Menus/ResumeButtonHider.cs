using UnityEngine;
using System.Collections;

public class ResumeButtonHider : MonoBehaviour {

    private PlayerController player;
    public GameObject resumeButton;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.isDead())
        {
            resumeButton.SetActive(false);
        }
        else
        {
            resumeButton.SetActive(true);
        }
	}
}
