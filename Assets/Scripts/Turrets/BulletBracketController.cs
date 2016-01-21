/*
 * Responsible for drawing the bullet brackets
 * Stops drawing them when the player is too far
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletBracketController : MonoBehaviour {

    private Transform player;
    private Image brackets;

    private float maxDistFromPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        brackets = GetComponentInChildren<Image>();
        
        maxDistFromPlayer = 10.0f; // To be adjusted later
    }

	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(player.position, transform.position) < maxDistFromPlayer)
        {
            transform.LookAt(Camera.main.transform, -Vector3.up);
            brackets.enabled = true;
        }
        else
        {
            brackets.enabled = false;
        }
    }
}
