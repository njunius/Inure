﻿/*
 * Bullet.cs
 * 
 * Defines functionality of individual bullet object:
 * - setting of color and velocity on instantiation
 * - reaction to collision with other GameObjects
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bullet : MonoBehaviour {

	//private Vector3 velocity;

    private int absorbValue;
    private int damage;

    private Image brackets;
    private ThreatTriggerController cachedTriggerLocation;

	// Use this for initialization
	void Awake () {
        brackets = GetComponentInChildren<Image>();
        absorbValue = 1;
        damage = 50;
	}
	
	// Update is called once per frame
	void Update () {

    }

    /*
	 * Description: Sets values for empty vars
	 * post: color and velocity of bullet are set
	 */
    public void setVars (Color bColor, Vector3 newVel) {
		gameObject.GetComponent<Renderer> ().material.color = bColor;
        gameObject.GetComponent<Rigidbody> ().velocity = newVel;

    }

    void OnCollisionEnter (Collision hit) {
		if (hit.gameObject.CompareTag("Player") && !hit.gameObject.GetComponent<PlayerController>().isShielded()) {
            // note that the 50 is a placeholder for real damage values later
            // and that the player's health is base 100 for future reference
            hit.gameObject.GetComponent<PlayerController>().takeDamage(damage);
		}
		if (!hit.gameObject.CompareTag("Projectile")) {
            if(cachedTriggerLocation != null && cachedTriggerLocation.getNumBullets() > 1)
            {
                cachedTriggerLocation.decrementBulletCount();
            }
			Destroy (gameObject);
		}
	}

    void OnTriggerEnter(Collider volume)
    {
        if (volume.gameObject.CompareTag("Warning Radius"))
        {
            brackets.enabled = true;
        }

        if (volume.gameObject.CompareTag("Threat Quadrant"))
        {
            cachedTriggerLocation = volume.gameObject.GetComponent<ThreatTriggerController>();
            cachedTriggerLocation.incrementBulletCount();
        }
    }

    void OnTriggerExit(Collider volume)
    {
        if (volume.gameObject.CompareTag("Warning Radius"))
        {
            brackets.enabled = false;
        }

        if (volume.gameObject.CompareTag("Threat Quadrant"))
        {
            volume.gameObject.GetComponent<ThreatTriggerController>().decrementBulletCount();
        }
    }

    public int getAbsorbValue()
    {
        return absorbValue;
    }
}
