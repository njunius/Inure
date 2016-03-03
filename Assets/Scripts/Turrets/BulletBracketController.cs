/*
 * Responsible for drawing the bullet brackets
 * Stops drawing them when the player is too far
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletBracketController : MonoBehaviour {

    private Renderer brackets;

    //private float maxDistFromPlayer;

    void Start()
    {
        brackets = GetComponent<SpriteRenderer>();

        brackets.enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if(brackets.enabled)
            transform.LookAt(Camera.main.transform, Camera.main.transform.up);
    }
}
