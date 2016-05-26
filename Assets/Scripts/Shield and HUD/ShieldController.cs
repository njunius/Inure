/* Controls the shield collision volume and mesh visibility
 * Responsible for the logic of depleting and recharging the shield
 * Responsible for knowing when to charge the bomb
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShieldController : MonoBehaviour, HUDElement
{
    // shield fields
    private bool shieldEnabled;
    private bool shieldActive;
    public float maxShieldCharge, currShieldCharge;
    private float shieldRechargeAmount; // used for recharging the shield to full
    private float shieldDepleteAmount; // used for draining shield charge when player activates the shield
    private float shieldChargeDelay; // delay in number of seconds
    private float shieldChargeDelayTimer; // timer used to keep track of the delay from the shield being depleted before it starts recharging

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
    private HUDColorController hudColorController;
    private string hudElementName;
    public Image shieldOutline;

    private ParticleSystem shieldParticles;
    private Collider shieldCollider;

    //Audio
    public AudioClip shieldOnSound;
    public AudioClip shieldAlarm;
    public AudioClip shieldRechargeStart;
    public AudioClip shieldRechargeMain;
    public AudioClip shieldRechargeEnd;
    public AudioClip shieldHitSound;
    private AudioSource baseSoundSource;
    private AudioSource effectSoundSource;
    private bool recharged = true;

    // Use this for initialization
    void Start()
    {
        baseSoundSource = GetComponents<AudioSource>()[0];
        effectSoundSource = GetComponents<AudioSource>()[1];

        // shield initializations
        shieldEnabled = true;           //Disables Shield Recharge for Tutorial
        shieldActive = false;
        maxShieldCharge = currShieldCharge = 100f;
        shieldChargeDelay = 2.0f;
        shieldChargeDelayTimer = 0.0f;
        shieldDepleteAmount = -20;
        shieldRechargeAmount = 10;

        hudElementName = "shield";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        shieldOutline.color = hudColorController.getColorByString(hudElementName);

        shieldParticles = transform.FindChild("Shield Field").gameObject.GetComponent<ParticleSystem>();
        shieldCollider = GetComponent<Collider>();

        bomb = GameObject.FindGameObjectWithTag("Bomb");
        bombBehavior = bomb.GetComponent<BombController>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Shield Gauge");
        shieldGauge = new Image[temp.Length];
        for (int i = 0; i < shieldGauge.Length; ++i)
        {
            shieldGauge[i] = temp[i].GetComponent<Image>();
            shieldGauge[i].color = hudColorController.getColorByString(hudElementName);
        }
        particleSystems = new List<GameObject>();

        SetParticleColors();
    }

    // Update is called once per frame
    void Update()
    {


        if (shieldEnabled)
        {
            if (!shieldActive) // shield recharging
            {
                shieldParticles.maxParticles = 0;
                shieldCollider.enabled = false;

                if (shieldChargeDelay > shieldChargeDelayTimer && currShieldCharge == 0) // delays the start of the shield recharge by 2 seconds
                {
                    shieldChargeDelayTimer += Time.deltaTime;

                    if (shieldChargeDelayTimer > shieldChargeDelay)
                    {
                        baseSoundSource.Stop();
                        baseSoundSource.PlayOneShot(shieldRechargeMain);
                        effectSoundSource.PlayOneShot(shieldRechargeStart);
                        recharged = false;
                    }
                }
                else if(currShieldCharge < maxShieldCharge)
                {
                    currShieldCharge += shieldRechargeAmount * Time.deltaTime;
                    
                }

                if (currShieldCharge >= maxShieldCharge)
                {
                    currShieldCharge = maxShieldCharge;
                }
                if (!recharged && (maxShieldCharge - currShieldCharge) / shieldRechargeAmount <= shieldRechargeEnd.length)
                {
                    effectSoundSource.PlayOneShot(shieldRechargeEnd);
                    baseSoundSource.Stop();
                    recharged = true;
                }


                for (int i = 0; i < shieldGauge.Length; ++i)
                {
                    shieldGauge[i].fillAmount = currShieldCharge / maxShieldCharge;
                }
            }
            else // shield depleting 
            {
                shieldParticles.maxParticles = 1000;
                shieldCollider.enabled = true;

                if (currShieldCharge <= 0)
                {

                    shieldActive = false;
                    baseSoundSource.Stop();
                    baseSoundSource.PlayOneShot(shieldAlarm);
                    currShieldCharge = 0;
                    shieldChargeDelayTimer = 0.0f;
                }
                else if (currShieldCharge > 0)
                {
                    currShieldCharge += shieldDepleteAmount * Time.deltaTime;

                    //baseSoundSource.pitch = currShieldCharge / maxShieldCharge;
                }

                if (currShieldCharge < 0f)
                {
                    currShieldCharge = 0f;
                }

                for (int i = 0; i < shieldGauge.Length; ++i)
                {
                    shieldGauge[i].fillAmount = currShieldCharge / maxShieldCharge;
                }
            }
        }
        
    }

    public void UpdateColor()
    {
        for (int i = 0; i < shieldGauge.Length; ++i)
        {
            shieldGauge[i].color = hudColorController.getColorByString(hudElementName);
        }
        shieldOutline.color = hudColorController.getColorByString(hudElementName);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") && shieldActive)
        {
            baseSoundSource.PlayOneShot(shieldHitSound);
            bombBehavior.chargeBomb(other.gameObject.GetComponent<Bullet>().getAbsorbValue());
            other.gameObject.GetComponent<Bullet>().Destroy();

            GameObject collisionParticles = Instantiate(shieldCollisionParticles);
            collisionParticles.transform.parent = transform;
            //Find angle between all three directions, then rotate around forward, then right, then up
            collisionParticles.transform.position = transform.position + new Vector3(0f, 7.5f, 0f);
            Vector3 centerToCollision = other.transform.position - transform.position;
            float angleForward = Vector3.Angle(transform.up, centerToCollision);
            float angleRight = Vector3.Angle(transform.right, centerToCollision);
            float angleUp = Vector3.Angle(transform.up, centerToCollision);
            collisionParticles.transform.RotateAround(transform.position, transform.forward, angleForward);
            collisionParticles.transform.RotateAround(transform.position, transform.right, angleRight);
            collisionParticles.transform.RotateAround(transform.position, transform.up, angleUp);
            for (int numTail = 0; numTail < collisionParticles.transform.childCount; ++numTail)
            {
                ParticleSystem tail = collisionParticles.transform.GetChild(numTail).GetChild(0).gameObject.GetComponent<ParticleSystem>();
                var color = tail.colorOverLifetime;
                Color bulletColor = other.gameObject.GetComponent<Renderer>().material.color;

                if (bulletColor == Color.red)
                    color.color = new ParticleSystem.MinMaxGradient(redTail);
                else if (bulletColor == Color.green)
                    color.color = new ParticleSystem.MinMaxGradient(greenTail);
                else if (bulletColor == Color.blue)
                    color.color = new ParticleSystem.MinMaxGradient(blueTail);
                else if (bulletColor == Color.cyan)
                    color.color = new ParticleSystem.MinMaxGradient(cyanTail);
                else if (bulletColor == Color.magenta)
                    color.color = new ParticleSystem.MinMaxGradient(magentaTail);
                else if (bulletColor == Color.yellow)
                    color.color = new ParticleSystem.MinMaxGradient(yellowTail);
                else if (bulletColor == new Color(1f, 0.6f, 0f, 1f))
                    color.color = new ParticleSystem.MinMaxGradient(orangeTail);
            }
            collisionParticles.GetComponent<ParticleSystem>().Play();

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

    public float getCurrShieldCharge()
    {
        return currShieldCharge;
    }

    public void setCurrShieldCharge(float setShield)
    {
        if(setShield > maxShieldCharge)
        {
            currShieldCharge = maxShieldCharge;
        }
        else if(setShield < 0)
        {
            currShieldCharge = 0;
        }
        else
        {
            currShieldCharge = setShield;
        }

        for (int i = 0; i < shieldGauge.Length; ++i)
        {
            shieldGauge[i].fillAmount = currShieldCharge;
        }
    }

    public void setShieldActive(bool setActive)
    {
        shieldActive = setActive;
        if (setActive)
        {
            baseSoundSource.PlayOneShot(shieldOnSound);
        }

    }

    public void setShieldEnabled(bool setEnabled)
    {
        shieldEnabled = setEnabled;
        if (setEnabled == false)
        {
            currShieldCharge = 0;
            shieldParticles.maxParticles = 0;
            shieldCollider.enabled = false;
        }
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

    private IEnumerator DestroyParticles(GameObject particles, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        particles.transform.parent = null;
        Destroy(particles);
        //ParticlePooler.current.ReassignTransform (particles);
        //particles.SetActive (false);
    }

    private void SetParticleColors()
    {
        redTail = new Gradient();
        redTail.SetKeys(new GradientColorKey[] {
            new GradientColorKey (Color.white, 0.0f),
            new GradientColorKey (new Color (0.97f, 0.25f, 0.25f), 0.21f),
            new GradientColorKey (new Color(0.51f, 0.06f, 0.06f), 0.57f),
            new GradientColorKey (Color.white, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey (1.0f, 0.0f),
            new GradientAlphaKey (1.0f, 0.76f),
            new GradientAlphaKey (0.0f, 1.0f)
        });
        greenTail = new Gradient();
        greenTail.SetKeys(new GradientColorKey[] {
            new GradientColorKey (Color.white, 0.0f),
            new GradientColorKey (new Color (0.25f, 0.97f, 0.34f), 0.21f),
            new GradientColorKey (new Color(0.12f, 0.72f, 0.47f), 0.57f),
            new GradientColorKey (Color.white, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey (1.0f, 0.0f),
            new GradientAlphaKey (1.0f, 0.76f),
            new GradientAlphaKey (0.0f, 1.0f)
        });
        blueTail = new Gradient();
        blueTail.SetKeys(new GradientColorKey[] {
            new GradientColorKey (Color.white, 0.0f),
            new GradientColorKey (new Color (0.25f, 0.25f, 0.97f), 0.21f),
            new GradientColorKey (new Color(0.04f, 0.04f, 0.38f), 0.57f),
            new GradientColorKey (Color.white, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey (1.0f, 0.0f),
            new GradientAlphaKey (1.0f, 0.76f),
            new GradientAlphaKey (0.0f, 1.0f)
        });
        cyanTail = new Gradient();
        cyanTail.SetKeys(new GradientColorKey[] {
            new GradientColorKey (Color.white, 0.0f),
            new GradientColorKey (new Color (0.25f, 0.82f, 0.97f), 0.21f),
            new GradientColorKey (new Color(0.19f, 0.23f, 0.63f), 0.57f),
            new GradientColorKey (Color.white, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey (1.0f, 0.0f),
            new GradientAlphaKey (1.0f, 0.76f),
            new GradientAlphaKey (0.0f, 1.0f)
        });
        magentaTail = new Gradient();
        magentaTail.SetKeys(new GradientColorKey[] {
            new GradientColorKey (Color.white, 0.0f),
            new GradientColorKey (new Color (0.97f, 0.25f, 0.95f), 0.21f),
            new GradientColorKey (new Color(0.38f, 0.04f, 0.38f), 0.57f),
            new GradientColorKey (Color.white, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey (1.0f, 0.0f),
            new GradientAlphaKey (1.0f, 0.76f),
            new GradientAlphaKey (0.0f, 1.0f)
        });
        yellowTail = new Gradient();
        yellowTail.SetKeys(new GradientColorKey[] {
            new GradientColorKey (Color.white, 0.0f),
            new GradientColorKey (new Color (0.97f, 0.95f, 0.25f), 0.21f),
            new GradientColorKey (new Color(0.38f, 0.34f, 0.04f), 0.57f),
            new GradientColorKey (Color.white, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey (1.0f, 0.0f),
            new GradientAlphaKey (1.0f, 0.76f),
            new GradientAlphaKey (0.0f, 1.0f)
        });
        orangeTail = new Gradient();
        orangeTail.SetKeys(new GradientColorKey[] {
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
