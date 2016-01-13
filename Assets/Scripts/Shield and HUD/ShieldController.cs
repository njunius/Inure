﻿using UnityEngine;
using System.Collections;


// Controls the shield collision volume and mesh visibility as well as the logic for using the shield
public class ShieldController : MonoBehaviour {
    // shield elements (made private later)
    public bool shieldActive;
    public int maxShieldCharge, currShieldCharge;
    public int shieldRechargeAmount; // used for recharging the shield to full
    public int shieldDepleteAmount; // used for draining shield charge when player activates the shield
    public float shieldChargeDelay; // delay in number of seconds
    public float shieldChargeDelayTimer; // timer used to keep track of the delay from the shield being depleted before it starts recharging
    public float shieldDeltaChargeTimer; // timer for delaying each change in the shield value

    // Use this for initialization
    void Start() {
        // shield initializations
        shieldActive = false;
        maxShieldCharge = currShieldCharge = 100;
        shieldChargeDelay = 2.0f;
        shieldChargeDelayTimer = 0.0f;
        shieldDepleteAmount = -20;
        shieldRechargeAmount = 5;
        shieldDeltaChargeTimer = 0.0f;
    }

    // Update is called once per frame
    void Update() {

        // enables and disables the effect and collision volume based on the player's state
        if (shieldActive)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!shieldActive) // shield recharging
        {
            if (shieldChargeDelay > shieldChargeDelayTimer && currShieldCharge < maxShieldCharge) // delays the start of the shield recharge by 2 seconds
            {
                shieldChargeDelayTimer += Time.deltaTime;
            }
            else if (currShieldCharge < maxShieldCharge && shieldDeltaChargeTimer >= 1.0f) // add a charge to the shield after a 1 second delay
            {
                currShieldCharge += shieldRechargeAmount;
                shieldDeltaChargeTimer = 0.0f;
            }
            else if (shieldDeltaChargeTimer < 1.0f)
            {
                shieldDeltaChargeTimer += Time.deltaTime;
            }
        }
        else // shield depleting
        {
            if (currShieldCharge > 0 && shieldDeltaChargeTimer >= 1.0f) // remove a charge from the shield after a 1 second delay
            {
                currShieldCharge += shieldDepleteAmount;
                shieldDeltaChargeTimer = 0.0f;
            }
            else if (shieldDeltaChargeTimer < 1.0f)
            {
                shieldDeltaChargeTimer += Time.deltaTime;
            }
            else if (currShieldCharge <= 0)
            {
                shieldActive = !shieldActive;
                shieldChargeDelayTimer = 0.0f;
            }
        }
    }

    public bool getShieldActive()
    {
        return shieldActive;
    }

    public bool isShieldCharged()
    {
        return currShieldCharge == maxShieldCharge;
    }

    public void setShieldActive(bool setActive)
    {
        shieldActive = setActive;
    }
}