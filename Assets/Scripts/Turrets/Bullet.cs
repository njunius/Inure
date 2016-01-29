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
    private ThreatTriggerController[] cachedTrigger; // can store up to all 4 quadrants behind the player if need be

	// Use this for initialization
	void Awake () {
        brackets = GetComponentInChildren<Image>();
        absorbValue = 1;
        damage = 50;

        cachedTrigger = new ThreatTriggerController[4];
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

        // go through all cached triggers and remove the bullet from the count before destruction
        for(int i = 0; i < cachedTrigger.Length; ++i)
        {
            if (cachedTrigger[i] != null && cachedTrigger[i].getNumBullets() > 0)
            {
                cachedTrigger[i].decrementBulletCount();
            }

        }

        Destroy (gameObject);
	}

    void OnTriggerEnter(Collider volume)
    {
        if (volume.gameObject.CompareTag("Warning Radius"))
        {
            brackets.enabled = true;
        }

        if(volume.gameObject.CompareTag("Threat Quadrant"))
        {
            // check to see if a quadrant has already been cached
            // if not add it
            // increment the quadrant's count
            for (int i = 0; i < cachedTrigger.Length; ++i)
            {
                if(cachedTrigger[i] == volume.gameObject.GetComponent<ThreatTriggerController>())
                {
                    cachedTrigger[i].incrementBulletCount();
                    break;
                }
                if(cachedTrigger[i] == null)
                {
                    cachedTrigger[i] = volume.gameObject.GetComponent<ThreatTriggerController>();
                    cachedTrigger[i].incrementBulletCount();
                    break;
                }
            }
        }

    }

    void OnTriggerExit(Collider volume)
    {
        if (volume.gameObject.CompareTag("Warning Radius"))
        {
            brackets.enabled = false;
        }

        if (volume.gameObject.CompareTag("Threat Quadrant") && volume.gameObject.GetComponent<ThreatTriggerController>().getNumBullets() > 0)
        {
            volume.gameObject.GetComponent<ThreatTriggerController>().decrementBulletCount();
        }
    }

    public int getAbsorbValue()
    {
        return absorbValue;
    }
}
