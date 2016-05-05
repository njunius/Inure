﻿/* Controls the shield collision volume and mesh visibility
 * Responsible for the logic of depleting and recharging the shield
 * Responsible for knowing when to charge the bomb
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShieldController : MonoBehaviour {
    // shield fields
    private bool shieldActive;
    public int maxShieldCharge, currShieldCharge;
    private int shieldRechargeAmount; // used for recharging the shield to full
    private int shieldDepleteAmount; // used for draining shield charge when player activates the shield
    private float shieldChargeDelay; // delay in number of seconds
    private float shieldChargeDelayTimer; // timer used to keep track of the delay from the shield being depleted before it starts recharging
    private float shieldDeltaChargeTimer; // timer for delaying each change in the shield value
	private List<GameObject> particleSystems;
	public GameObject shieldCollisionParticles;
	private Gradient redTail;
	private Gradient greenTail;
	private Gradient blueTail;
	private Gradient cyanTail;
	private Gradient magentaTail;
	private Gradient yellowTail;
	private Gradient orangeTail;

    // Shield Gauge fields
    private Image[] shieldGauge;
    private GameObject bomb;
    private BombController bombBehavior;
    private PlayerController player;
    //private MeshRenderer shieldMesh;
	private ParticleSystem shieldParticles;
    private Collider shieldCollider;
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
        shieldDeltaChargeTimer = 0.0f; // initialize to 0 otherwise the timer is immediately updated.
        interpShieldValue = 100.0f;

        //shieldMesh = GetComponent<MeshRenderer>();
		shieldParticles = transform.FindChild("Shield Field").gameObject.GetComponent<ParticleSystem> ();
        shieldCollider = GetComponent<Collider>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        bomb = GameObject.FindGameObjectWithTag("Bomb");
        bombBehavior = bomb.GetComponent<BombController>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Shield Gauge");
        shieldGauge = new Image[temp.Length];
        for(int i = 0; i < shieldGauge.Length; ++i)
        {
            shieldGauge[i] = temp[i].GetComponent<Image>();
        }
		particleSystems = new List<GameObject> ();

		SetParticleColors ();
    }

    // Update is called once per frame
    void Update() {
        if (!player.paused)
        {

            if (!shieldActive) // shield recharging
            {

                //shieldMesh.enabled = false;
				shieldParticles.maxParticles = 0;
                shieldCollider.enabled = false;

                if (currShieldCharge > maxShieldCharge)
                {
                    currShieldCharge = maxShieldCharge;
                }
                else if (shieldChargeDelay > shieldChargeDelayTimer && currShieldCharge == 0) // delays the start of the shield recharge by 2 seconds
                {
                    shieldChargeDelayTimer += Time.deltaTime;
                    interpShieldValue = 0.0f;
                }
                else if (currShieldCharge < maxShieldCharge && shieldDeltaChargeTimer >= 1.0f) // add a charge to the shield after a 1 second delay
                {
                    currShieldCharge += shieldRechargeAmount;
                    shieldDeltaChargeTimer = 0.0f;
                }
                else if (shieldDeltaChargeTimer < 1.0f && currShieldCharge < maxShieldCharge)
                {
                    shieldDeltaChargeTimer += Time.deltaTime;
                }

                if (currShieldCharge < maxShieldCharge)
                {
                    interpShieldValue += shieldRechargeAmount * Time.deltaTime;

                    if (interpShieldValue > maxShieldCharge)
                    {
                        interpShieldValue = (float)maxShieldCharge;
                    }

                    for (int i = 0; i < shieldGauge.Length; ++i)
                    {
                        shieldGauge[i].fillAmount = interpShieldValue / (float)maxShieldCharge;
                    }
                }
                
            }
            else // shield depleting
            {

                //shieldMesh.enabled = true;
				shieldParticles.maxParticles = 1000;
                shieldCollider.enabled = true;

                if (currShieldCharge <= 0)
                {
                    shieldActive = false;
                    currShieldCharge = 0;
                    shieldChargeDelayTimer = 0.0f;
                    shieldDeltaChargeTimer = 1.0f; // set to 1 to allow the shield to be immediately charged, otherwise there is a 3 second delay instead of a 2 second one
                }
                else if (currShieldCharge > 0 && shieldDeltaChargeTimer >= 1.0f) // remove a charge from the shield after a 1 second delay
                {
                    currShieldCharge += shieldDepleteAmount;
                    shieldDeltaChargeTimer = 0.0f;
                }
                else if (shieldDeltaChargeTimer < 1.0f)
                {
                    shieldDeltaChargeTimer += Time.deltaTime;
                } 
                
                interpShieldValue += shieldDepleteAmount * Time.deltaTime;

                if (interpShieldValue < 0f)
                {
                    interpShieldValue = 0f;
                }

                for (int i = 0; i < shieldGauge.Length; ++i)
                {
                    shieldGauge[i].fillAmount = interpShieldValue / (float)maxShieldCharge;
                }

            }
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {
		if (collision.collider.gameObject.CompareTag("Projectile") && shieldActive)
        {
            bombBehavior.chargeBomb(collision.collider.gameObject.GetComponent<Bullet>().getAbsorbValue());
        }
		Transform collisionParticlesTransform = gameObject.transform.FindChild ("Bullet Collision Particles");
		//Vector3.RotateTowards(collisionParticles.transform.up, collision.)
    } */

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Projectile") && shieldActive) {
			bombBehavior.chargeBomb (other.gameObject.GetComponent<Bullet> ().getAbsorbValue ());
			other.gameObject.GetComponent<Bullet> ().Destroy ();

			GameObject collisionParticles = Instantiate(shieldCollisionParticles);
			collisionParticles.transform.parent = transform;
			//Find angle between all three directions, then rotate around forward, then right, then up
			collisionParticles.transform.position = transform.position + new Vector3(0f, 7.5f, 0f);
			Vector3 centerToCollision = other.transform.position - transform.position;
			float angleForward = Vector3.Angle (transform.up, centerToCollision);
			float angleRight = Vector3.Angle (transform.right, centerToCollision);
			float angleUp = Vector3.Angle (transform.up, centerToCollision);
			collisionParticles.transform.RotateAround(transform.position, transform.forward, angleForward);
			collisionParticles.transform.RotateAround(transform.position, transform.right, angleRight);
			collisionParticles.transform.RotateAround(transform.position, transform.up, angleUp);
			for (int numTail = 0; numTail < collisionParticles.transform.childCount; ++numTail) {
				ParticleSystem tail = collisionParticles.transform.GetChild (numTail).GetChild (0).gameObject.GetComponent<ParticleSystem> ();
				var color = tail.colorOverLifetime;
				Color bulletColor = other.gameObject.GetComponent<Renderer> ().material.color;

				if (bulletColor == Color.red)
					color.color = new ParticleSystem.MinMaxGradient (redTail);
				else if (bulletColor == Color.green)
					color.color = new ParticleSystem.MinMaxGradient (greenTail);
				else if (bulletColor == Color.blue)
					color.color = new ParticleSystem.MinMaxGradient (blueTail);
				else if (bulletColor == Color.cyan)
					color.color = new ParticleSystem.MinMaxGradient (cyanTail);
				else if (bulletColor == Color.magenta)
					color.color = new ParticleSystem.MinMaxGradient (magentaTail);
				else if (bulletColor == Color.yellow)
					color.color = new ParticleSystem.MinMaxGradient (yellowTail);
				else if (bulletColor == new Color (1f, 0.6f, 0f, 1f))
					color.color = new ParticleSystem.MinMaxGradient (orangeTail);
			}
			collisionParticles.GetComponent<ParticleSystem> ().Play ();

			StartCoroutine(DestroyParticles(collisionParticles, 2f));
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

	public int getCurrShieldCharge ()
    {
		return currShieldCharge;
	}

	public void setCurrShieldCharge (int setShield)
    {
		currShieldCharge = setShield;

        interpShieldValue = (float)currShieldCharge;

        for(int i = 0; i < shieldGauge.Length; ++i)
        {
            shieldGauge[i].fillAmount = currShieldCharge;
        }
	}

    public void setShieldActive(bool setActive)
    {
        shieldActive = setActive;
    }

	/*private GameObject GetParticleSystem () {
		Debug.Log ("Two");
		GameObject particles = ParticlePooler.current.GetPooledParticle ();
		particles.transform.parent = transform;
		particles.transform.position = particles.transform.parent.transform.position;
		Debug.Log ("Three");

		if (particleSystems.Contains (particles)) {
			Debug.Log ("TWO, " + particles.transform.position + ", " + particles.transform.rotation);
		} else {
			Debug.Log ("ONE, " + particles.transform.position + ", " + particles.transform.rotation);
			particleSystems.Add (particles);
		}

		return particles;
	}*/

	private IEnumerator DestroyParticles (GameObject particles, float delayTime) {
		yield return new WaitForSeconds (delayTime);
		particles.transform.parent = null;
		Destroy (particles);
		//ParticlePooler.current.ReassignTransform (particles);
		//particles.SetActive (false);
	}

	private void SetParticleColors () {
		redTail = new Gradient ();
		redTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.97f, 0.25f, 0.25f), 0.21f),
			new GradientColorKey (new Color(0.51f, 0.06f, 0.06f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
		greenTail = new Gradient ();
		greenTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.25f, 0.97f, 0.34f), 0.21f),
			new GradientColorKey (new Color(0.12f, 0.72f, 0.47f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
		blueTail = new Gradient ();
		blueTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.25f, 0.25f, 0.97f), 0.21f),
			new GradientColorKey (new Color(0.04f, 0.04f, 0.38f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
		cyanTail = new Gradient ();
		cyanTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.25f, 0.82f, 0.97f), 0.21f),
			new GradientColorKey (new Color(0.19f, 0.23f, 0.63f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
		magentaTail = new Gradient ();
		magentaTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.97f, 0.25f, 0.95f), 0.21f),
			new GradientColorKey (new Color(0.38f, 0.04f, 0.38f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
		yellowTail = new Gradient ();
		yellowTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.97f, 0.95f, 0.25f), 0.21f),
			new GradientColorKey (new Color(0.38f, 0.34f, 0.04f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
		orangeTail = new Gradient ();
		orangeTail.SetKeys (new GradientColorKey[] {
			new GradientColorKey (Color.white, 0.0f),
			new GradientColorKey (new Color (0.86f, 0.52f, 0.0f), 0.21f),
			new GradientColorKey (new Color(0.38f, 0.27f, 0.04f), 0.57f),
			new GradientColorKey (Color.white, 1.0f)
		}, new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f),
			new GradientAlphaKey (1.0f, 0.76f),
			new GradientAlphaKey (0.0f, 1.0f)
		});
	}
}
