/* Controls the shield collision volume and mesh visibility
 * Responsible for the logic of depleting and recharging the shield
 * Responsible for knowing when to charge the bomb
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour {
    // shield fields
    private bool shieldActive;
    private int maxShieldCharge, currShieldCharge;
    private int shieldRechargeAmount; // used for recharging the shield to full
    private int shieldDepleteAmount; // used for draining shield charge when player activates the shield
    private float shieldChargeDelay; // delay in number of seconds
    private float shieldChargeDelayTimer; // timer used to keep track of the delay from the shield being depleted before it starts recharging
    private float shieldDeltaChargeTimer; // timer for delaying each change in the shield value

    // Shield Gauge fields
    private Image[] shieldGauge;
    private GameObject bomb;
    private BombController bombBehavior;
    private float interpShieldValue;

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
        interpShieldValue = 100.0f;

        bomb = GameObject.FindGameObjectWithTag("Bomb");
        bombBehavior = bomb.GetComponent<BombController>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Shield Gauge");
        shieldGauge = new Image[temp.Length];
        for(int i = 0; i < shieldGauge.Length; ++i)
        {
            shieldGauge[i] = temp[i].GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update() {

        // enables and disables the effect and collision volume based on the player's state
        if (shieldActive)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshCollider>().enabled = true;

            interpShieldValue += shieldDepleteAmount * Time.deltaTime;
            for (int i = 0; i < shieldGauge.Length; ++i)
            {
                shieldGauge[i].fillAmount = interpShieldValue / (float)maxShieldCharge;
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;

            if (currShieldCharge < maxShieldCharge)
            {
                interpShieldValue += shieldRechargeAmount * Time.deltaTime;
                for (int i = 0; i < shieldGauge.Length; ++i)
                {
                    shieldGauge[i].fillAmount = interpShieldValue / (float)maxShieldCharge;
                }
            }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") && shieldActive)
        {
            bombBehavior.chargeBomb(other.gameObject.GetComponent<Bullet>().getAbsorbValue());
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
