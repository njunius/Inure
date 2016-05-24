/*
 * Controls the player's physical movement
 * Responsible for interpreting the input manager into player actions during gameplay
 * Responsible for keeping track of player's hull integrity
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 200.0f;
    public float maxSpeed = 200.0f;
    public float rotSpeed = 120.0f;
    public float rollSpeed = 100.0f;
    public Color bulletColor = Color.blue;
	public float bulletVel = 220.0f;
	public float fireRate = 0.2f;
	private float useGaugeIncreaseRate = 10f;
	public GameObject bulletPrefab;
	public GameObject powerBomb;

    public GameObject bulletSpawns;
    public Transform[] bulletSpawnLocations;
    int bulletSpawnLocIndex;
	public GameObject powerBombSpawner;

    public float invulnSecs = 1.0f;
	public bool noGameOver = false;

	private Vector3 frontOfShip;
	private bool isFiring = false;
	private bool isSlowed = false;
    private bool invincibleFlashing = false;
	private bool justUsedBomb = false;

    public bool paused = false;

    // player armor/health stats
    private int maxHullIntegrity;
    private int currHullIntegrity;
	private bool fInvincible = false;
	private string[] powerUpList = new string[]{"", "PowerUp_EMP", "PowerUp_Shockwave", "PowerUp_SlowTime"};
	private string curPowerUp;
    private ArmorController armorGauge;

    private BombCountdownController bombTimer;

    private Rigidbody rb;

    private Canvas pauseScreen; //Base user interface, pause menu here

	private float timerTMP = 0;
    
	private ShieldController shield;
	private BombController bomb;

    public GameObject lockOnTarget;
    public bool targetLocked = false;

    public GameObject mesh;
    private Color originalColor;

    public GameObject gameController;
    public InputManager im;

    public bool wallSlide = true;

    private bool turned = false;
    private Vector3 localPrevVel = Vector3.zero;

	public ParticleSystem mainThrusterLeft;
	public ParticleSystem mainThrusterRight;
	public GameObject thrusterLeft;
	public GameObject thrusterRight;
	public GameObject contrail;
	private GameObject trailLeft;
	private GameObject trailRight;
	public Vector3 trailPositionLeft = new Vector3 (-5.5f, -3f, 0f);
	public Vector3 trailPositionRight = new Vector3 (5.5f, -3f, 0f);

    //sounds
    public AudioClip playerBulletSound;
    public AudioClip hullRestoreSound;
    public AudioClip hullDamageSound;
    public AudioClip deathSound;
    public AudioClip ShieldHitSound;
    public AudioClip SteeringSound;
    public AudioClip ThrottleUpSound;
    public AudioClip PowerDownSound;
    public AudioClip ScrapeHullSound;
    public AudioClip BombChargeSound;
    public AudioClip BombLaunchSound;

    private AudioSource audio_bullet;
    private AudioSource audio_engineHum;
    private AudioSource audio_accellerators;
    private float audio_accellerators_max_vol;
    private AudioSource audio_thrusters;
    private float audio_thrusters_max_vol;
    private AudioSource audio_hullHit;
    private AudioSource audio_wallImpact;
    private AudioSource audio_effects;
    private AudioSource audio_bomb;

    public float thrustersWarmUpTime = 1.0f;
    private float thrustersEnergy = 0;

    public float thrustersCoolDownTime = 1.0f;

    private float wallScrapeTime = 0.5f;

    private float volLowRange = 0.7f;
    private float volHighRange = 1.0f;

    private bool hasStopped = true;

    public bool rotateEnabled = true;
    public bool verticalEnginesEnabled = true;
    public bool longitudinalEnginesEnabled = true;
    public bool lateralEnginesEnabled = true;
    public bool weaponsEnabled = true;
    public bool shieldEnabled = true;

    public bool tutorialMode = false;

    public bool twoSpeedMode = false;
    public float twoSpeedMultiplier = 2;
    public float twoSpeedTimer = 5;
    private float speedTimer = 0;
    private bool twoSpeedEngaged = false;

    private bool tutorialInitialized = false;

    // Use this for initialization
    void Awake () {
		Cursor.lockState = CursorLockMode.Confined;
        audio_bullet = GetComponents<AudioSource>()[0];   //0: bullets, 1: engines, 2: shield, 3: impacts, 4: other
        audio_engineHum = GetComponents<AudioSource>()[1];
        audio_accellerators = GetComponents<AudioSource>()[2];
        audio_thrusters = GetComponents<AudioSource>()[3];
        audio_hullHit = GetComponents<AudioSource>()[4];
        audio_wallImpact = GetComponents<AudioSource>()[5];
        audio_effects = GetComponents<AudioSource>()[6];
        audio_bomb = GetComponents<AudioSource>()[7];
        audio_accellerators_max_vol = audio_accellerators.volume;
        audio_thrusters_max_vol = audio_thrusters.volume;
        audio_thrusters.volume = 0;
        Time.timeScale = 1; // The time scale must be reset upon loading from the main menu

        rb = GetComponent<Rigidbody>();
        originalColor = mesh.GetComponent<Renderer>().material.color;

        curPowerUp = powerUpList[0];

        pauseScreen = GameObject.FindGameObjectWithTag("Pause Overlay").GetComponent<Canvas>();

        shield = GetComponentInChildren<ShieldController>();
		bomb = GetComponentInChildren<BombController> ();

        maxHullIntegrity = currHullIntegrity = 5;
        armorGauge = gameObject.GetComponentInChildren<ArmorController>();

        bombTimer = gameObject.GetComponentInChildren<BombCountdownController>();

        if (tutorialMode)
        {
            Debug.Log("tutorial mode");
            currHullIntegrity = 0;
        }

        Transform[] temp = bulletSpawns.GetComponentsInChildren<Transform>();
        bulletSpawnLocations = new Transform[temp.Length - 1];
        bulletSpawnLocIndex = 0;
        for(int i = 0; i < temp.Length; ++i)
        {
            if(temp[i].gameObject.GetInstanceID() != bulletSpawns.GetInstanceID())
            {
                bulletSpawnLocations[bulletSpawnLocIndex] = temp[i];
                ++bulletSpawnLocIndex;
            }
        }

        if (GameObject.Find("GameController") == null)
        {
            Instantiate(gameController);         
        }
        gameController = GameObject.FindGameObjectWithTag("GameController");
        im = gameController.GetComponent<InputManager>();

		mainThrusterLeft.Play ();
		mainThrusterRight.Play ();
    }

    // Update is called once per frame
    void Update () {
        if (tutorialMode && !tutorialInitialized)
        {
            shield.setCurrShieldCharge(0);
            shield.setShieldEnabled(false);
            tutorialInitialized = true;
        }

        if (!paused)
        {
            //find new point at front of ship for firing
            Vector3 forwardNorm = gameObject.transform.forward;
            forwardNorm.Normalize();
            frontOfShip = mesh.GetComponent<Renderer>().bounds.center + (forwardNorm * mesh.GetComponent<Renderer>().bounds.extents.z * 1.15f);

            //Activate the game over sequence when death is true
            if (!tutorialMode && isDead() && !noGameOver)
            {
                //killPlayer();
            }
            //Count down invulnerability
            if (fInvincible)
            {
                timerTMP -= Time.deltaTime;
                if (timerTMP / invulnSecs < .8)
                {
                    if (!invincibleFlashing)
                    {
                        Renderer r = mesh.GetComponent<Renderer>();
                        r.material.color = originalColor;
                        r.material.DisableKeyword("_Emmisive");
                        invincibleFlashing = true;
                    }
                    else
                    {
                        Renderer r = mesh.GetComponent<Renderer>();
                        if (r.enabled)
                        {
                            r.enabled = false;
                        }
                        else
                        {
                            r.enabled = true;
                        }

                    }

                }

                if (timerTMP <= 0)
                {
                    fInvincible = false;
                    invincibleFlashing = false;
                    Renderer r = mesh.GetComponent<Renderer>();
                    r.material.color = originalColor;
                    r.enabled = true;
                }
            }

            // Shield Controls 
            if (shieldEnabled && im.getInput("Shield") > 0.3f && !shield.getShieldActive() && shield.isShieldCharged())
            {
                shield.setShieldActive(true);
            }

            // Shooting controls
            if (weaponsEnabled && im.getInput("Shoot") > 0.3f && !isFiring)
            {
                isFiring = true;
                InvokeRepeating("fireBullets", 0.0f, fireRate);
            }

            if (weaponsEnabled && im.getInputUpEnhanced("Shoot"))
            {
                CancelInvoke("fireBullets");
                isFiring = false;
            }

            /*if (im.getInputDown("Use Powerup") && curPowerUp != "")
            {
                PowerUp whichPowerUp = null;
                switch (curPowerUp)
                {
                    case "PowerUp_EMP":
                        GetComponent<PowerUp_EMP>().enabled = true;
                        GetComponent<PowerUp_EMP>().Activate();
                        whichPowerUp = GetComponent<PowerUp_EMP>();
                        break;
                    case "PowerUp_Shockwave":
                        GetComponent<PowerUp_Shockwave>().enabled = true;
                        GetComponent<PowerUp_Shockwave>().Activate();
                        whichPowerUp = GetComponent<PowerUp_Shockwave>();
                        break;
                    case "PowerUp_SlowTime":
                        GetComponent<PowerUp_SlowTime>().enabled = true;
                        GetComponent<PowerUp_SlowTime>().Activate();
                        whichPowerUp = GetComponent<PowerUp_SlowTime>();
                        break;
                    default:
                        break;
                }

                if (whichPowerUp != null)
                {
                    //whichPowerUp.enabled = false;
                }

                //gameObject.GetComponent<PowerUp> ().Activate ();
                //Destroy(gameObject.GetComponent<PowerUp> ());
                curPowerUp = "";
            }*/

			if (im.getInput("Launch Bomb") > 0.3) {
				if (getBombCharge () > 0) {
                    if (!audio_bomb.isPlaying)
                    {
                        audio_bomb.loop = true;
                        audio_bomb.PlayOneShot(BombChargeSound);
                    }
					float newCharge = (getUseCharge () + (useGaugeIncreaseRate * Time.deltaTime));
					setUseCharge(Mathf.Min (newCharge, getBombCharge ()));
					if (getUseCharge () == getBombCharge ()) {
						FireBomb ((int)Mathf.Floor(getUseCharge ()));
						justUsedBomb = true;
					}
					useGaugeIncreaseRate += (5f * Time.deltaTime);
				}
			}
			if (im.getInputUp("Launch Bomb")) {
				if (getBombCharge () > 0 && !justUsedBomb) {
					FireBomb ((int)Mathf.Floor(getUseCharge ()));
                    audio_bomb.Stop();
                    audio_bomb.loop = false;
                    audio_bomb.PlayOneShot(BombLaunchSound);
                }
				justUsedBomb = false;
			}
        }        
    }

    void FixedUpdate()
    {

        if (!paused)
        {
            float moveLongitudinal = im.getInput("Longitudinal") * moveSpeed;
            if (twoSpeedEngaged && moveLongitudinal > 0)
            {
                moveLongitudinal *= twoSpeedMultiplier;
            }
            float moveLateral = im.getInput("Lateral") * moveSpeed;
            float moveVertical = im.getInput("Vertical") * moveSpeed;
            float rotRoll = im.getInput("Roll") * rollSpeed;
            float rotPitch = im.getInput("Pitch") * rotSpeed;
            float rotYaw = im.getInput("Yaw") * rotSpeed;

            if (rotateEnabled && (rotPitch != 0 || rotYaw != 0 || rotRoll != 0))
            {
                if (!audio_thrusters.isPlaying)
                {
                    audio_thrusters.volume = 0;
                    thrustersEnergy = 0;
                    
                }
                else
                {
                    if (thrustersEnergy < thrustersWarmUpTime)
                    {
                        thrustersEnergy += Time.deltaTime;
                        audio_thrusters.volume = thrustersEnergy / thrustersWarmUpTime;
                    }
                }
                
            }
            

            if (rotPitch == 0 && rotYaw == 0 && rotRoll == 0)
            {
                if (audio_thrusters.volume > 0)
                {
                    if (thrustersEnergy > 0)
                    {
                        thrustersEnergy -= Time.deltaTime * 2;
                        audio_thrusters.volume = thrustersEnergy / thrustersWarmUpTime;
                    }

                    
                }
            }

            if (audio_thrusters.volume > audio_thrusters_max_vol)
            {
                audio_thrusters.volume = audio_thrusters_max_vol;
            }

            if (!hasStopped)
            {
                if (moveLongitudinal == 0 && moveLateral == 0 && moveVertical == 0)
                {
                    audio_accellerators.PlayOneShot(PowerDownSound);
                    hasStopped = true;
                }
                else
                {
                    hasStopped = false;
                }
            }
                


            if (twoSpeedEngaged && moveLongitudinal > 0)
            {
                moveLongitudinal *= twoSpeedMultiplier;
                moveLateral /= 2;
                moveVertical /= 2;
                rotPitch /= 2;
                rotYaw /= 2;
            }

            if (twoSpeedMode)
            {
                if (moveLongitudinal <= 0)
                {
                    speedTimer = 0;
                }

            }

            if (!verticalEnginesEnabled)
            {
                moveVertical = 0;
            }
            if (!longitudinalEnginesEnabled)
            {
                moveLongitudinal = 0;
            }
            if (!lateralEnginesEnabled)
            {
                moveLateral = 0;
            }


            if (!rotateEnabled)
            {
                rotRoll = rotPitch = rotYaw = 0;
            }

            if (wallSlide)
            {
                if (im.getInputUpEnhanced("Longitudinal"))
                {
                    rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x,
                                                                       transform.InverseTransformDirection(rb.velocity).y, 0));
                }

                if (im.getInputUpEnhanced("Lateral"))
                {
                    rb.velocity = transform.TransformDirection(new Vector3(0, transform.InverseTransformDirection(rb.velocity).y,
                                                                            transform.InverseTransformDirection(rb.velocity).z));
                }
                if (im.getInputUpEnhanced("Vertical"))
                {
                    rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x, 0,
                                                                       transform.InverseTransformDirection(rb.velocity).z));
                }
            }

            if (moveLongitudinal == 0)
            {
                TurnOffThruster("main");
            }
            else if (moveLongitudinal != 0)
            {
                if (moveLongitudinal > 0)
                {
                    TurnOnThruster("main");
                }
                if (Mathf.Sign(transform.InverseTransformDirection(rb.velocity).z) != Mathf.Sign(moveLongitudinal))
                {
                    rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x,
                                                                   transform.InverseTransformDirection(rb.velocity).y, 0));
                }
                rb.AddForce(transform.TransformDirection(Vector3.forward * moveLongitudinal));
            }
            else if (!wallSlide)
            {
                rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x,
                                                                   transform.InverseTransformDirection(rb.velocity).y, 0));
            }

            if (moveLateral != 0)
            {
                if (Mathf.Sign(transform.InverseTransformDirection(rb.velocity).x) != Mathf.Sign(moveLateral))
                {
                    rb.velocity = transform.TransformDirection(new Vector3(0, transform.InverseTransformDirection(rb.velocity).y,
                                                                        transform.InverseTransformDirection(rb.velocity).z));
                }
                rb.AddForce(transform.TransformDirection(Vector3.right * moveLateral));
            }
            else if (!wallSlide)
            {
                rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x / 2, transform.InverseTransformDirection(rb.velocity).y,
                                                                        transform.InverseTransformDirection(rb.velocity).z));
            }

            if (moveVertical != 0)
            {
                if (Mathf.Sign(transform.InverseTransformDirection(rb.velocity).y) != Mathf.Sign(moveVertical))
                {
                    rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x, 0,
                                                                   transform.InverseTransformDirection(rb.velocity).z));
                }
                rb.AddForce(transform.TransformDirection(Vector3.up * moveVertical));
            }
            else if (!wallSlide)
            {
                rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x, transform.InverseTransformDirection(rb.velocity).y / 2,
                                                                   transform.InverseTransformDirection(rb.velocity).z));
            }

            if (moveLongitudinal == 0 && moveLateral == 0 && moveVertical == 0)
            {
                rb.velocity = Vector3.zero;
            }

            if (!targetLocked)
            {
                //localPrevVel = transform.InverseTransformVector(rb.velocity);
                localPrevVel = rb.velocity;
                if (rotPitch == 0)
                {
                    rb.angularVelocity = transform.TransformDirection(new Vector3(0, transform.InverseTransformDirection(rb.angularVelocity).y,
                                                                            transform.InverseTransformDirection(rb.angularVelocity).z));

                }

                if (rotYaw == 0)
                {
                    rb.angularVelocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x, 0,
                                                                       transform.InverseTransformDirection(rb.angularVelocity).z));
                }

                if (rotRoll == 0)
                {
                    rb.angularVelocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x,
                                                                       transform.InverseTransformDirection(rb.angularVelocity).y, 0));

                }
                if (rotPitch != 0 || rotYaw != 0 || rotRoll != 0)
                {
                    //rb.AddTorque(transform.TransformDirection(Vector3.left * rotPitch + Vector3.up * rotYaw + Vector3.forward * rotRoll));
                    rb.angularVelocity = transform.TransformDirection(Vector3.left * rotPitch + Vector3.up * rotYaw + Vector3.forward * rotRoll);
                    turned = true;
                }
                else
                {
                    rb.angularVelocity = Vector3.zero;
                    turned = false;
                }


            }
            else
            {
                if (lockOnTarget != null)
                {
                    Quaternion prevRot = transform.rotation;
                    transform.LookAt(lockOnTarget.transform, transform.up);

                    transform.rotation = Quaternion.RotateTowards(prevRot, transform.rotation, 2);

                    

                    if (Quaternion.Angle(prevRot, transform.rotation) != 0)
                    {
                        thrustersEnergy += Time.deltaTime;
                        if (thrustersEnergy > 1) thrustersEnergy = 1;
                        //Debug.Log(thrustersEnergy);
                        audio_thrusters.volume = thrustersEnergy;
                    }
                    else
                    {
                        thrustersEnergy -= Time.deltaTime * 2;
                        if (thrustersEnergy < 0) thrustersEnergy = 0;
                        audio_thrusters.volume = thrustersEnergy;
                    }
                    

                }
                if (rotRoll != 0)
                {
                    rb.angularVelocity = transform.TransformDirection(Vector3.left + Vector3.up + Vector3.forward * rotRoll);
                }
                else
                {
                    rb.angularVelocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x,
                                                                       transform.InverseTransformDirection(rb.angularVelocity).y, Time.deltaTime));

                }
            }


            float inGameMaxSpeed = maxSpeed;
            speedTimer += Time.deltaTime;
            if (twoSpeedMode  && speedTimer > twoSpeedTimer)
            {
                inGameMaxSpeed *= 1.1f;
                twoSpeedEngaged = true;
            }
            else
            {
                twoSpeedEngaged = false;
            }

            if (rb.velocity.magnitude > inGameMaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * inGameMaxSpeed;

            }
            if (rb.velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    void LateUpdate()
    {
        if (!targetLocked && turned)
        {
            //Debug.Log("Vel");
            //Debug.Log(rb.velocity);
            //Debug.Log(localPrevVel);
            //float vm = rb.velocity.magnitude;
            //rb.velocity = (rb.velocity - 0.9f * localPrevVel).normalized * vm;
            //rb.velocity = (rb.velocity + 2 * localPrevVel) / 3;
            //Debug.Log(rb.velocity);
        }
    }

	//Transports the player to the specified coordinates
	//Resets their stats to saved data
	//Usually called by a button in the Canvas UI
	public void reloadCheckP (LastCheckpoint savedData)
	{
        if (paused)
        {
            paused = false;
        }

        if(Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }

        if (pauseScreen.enabled)
        {
            pauseScreen.enabled = false;
        }

        Cursor.visible = false;

		//Teleport Player + Camera
		gameObject.transform.position = savedData.getCheckPOS();
		gameObject.transform.rotation = savedData.getCheckROT();

		GameObject.FindGameObjectWithTag("MainCamera").transform.position = savedData.getCheckCamPOS();
		GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = savedData.getCheckCamROT();

		//Reset stats
		currHullIntegrity = savedData.getHealth();
		shield.setCurrShieldCharge(savedData.getShield());
        armorGauge.updateChunks(currHullIntegrity);


        //Overwrite data
        //savePlayer();

		//Turn off turrets + Destroy bullets
		GameObject[] allTurrets, allBullets;
		allTurrets = GameObject.FindGameObjectsWithTag ("Turret");
		allBullets = GameObject.FindGameObjectsWithTag ("Projectile");
		for (int numTurret = 0; numTurret < allTurrets.Length; ++numTurret) {
			allTurrets [numTurret].GetComponent<Turret> ().TurnOff ();
		}

		/*for (int numBullet = 0; numBullet < allBullets.Length; ++numBullet) {
			Destroy(allBullets[numBullet]);
		}*/
	}


    private void resetMeshRotation()
    {
        mesh.transform.rotation = transform.rotation;
    }

	//Deactivates player controls and shows game over screen
	/*private void killPlayer()
	{
        audio_effects.PlayOneShot(deathSound);
        Cursor.visible = true;
        paused = true;
        //Show Game Over Screen
        Time.timeScale = 0.3f;
		pauseScreen.enabled = true;
		//Destroy player
		//this.enabled = false;
        
    }

	//Disable game over screen
	public void savePlayer ()
	{
        paused = false;
		//Show Pause Screen
		Time.timeScale = 1.0f;
		pauseScreen.enabled = false;
		//save player
		this.enabled = true;
	}*/

    // returns true if the player's hull integrity has dropped to 0
    public bool isDead()
    {
        return currHullIntegrity <= 0;
    }

    /* 
     * reduces the player's hull integrity by 1
     * NOTE: the player's health is the number of hits that can be taken
     */
	public void takeDamage()
    {
        if (!fInvincible)
        {
            audio_hullHit.PlayOneShot(hullDamageSound);
            --currHullIntegrity;
            
            //become fInvincible for invulnSecs
            timerTMP = invulnSecs;
            fInvincible = true;
            Renderer r = mesh.GetComponent<Renderer>();
            r.material.color = new Color(255, 255, 255, r.material.color.a);
            r.material.EnableKeyword("_Emmisive");

            armorGauge.updateChunks(currHullIntegrity);
        }

        if (currHullIntegrity < 0)
        {
            currHullIntegrity = 0;
        }

        if (bombTimer.isCountingDown() && currHullIntegrity == 0)
        {
            SceneManager.LoadScene("EndingDead");
        }
    }

    /*
     * newHullIntegrity cannot be greater than maxHullIntegrity
     * newHullIntegrity cannot be less than 0
     */
    public void setHullIntegrity(int newHullIntegrity)
    {
        if(newHullIntegrity > maxHullIntegrity)
        {
            currHullIntegrity = maxHullIntegrity;
        }
        else if(newHullIntegrity < 0)
        {
            currHullIntegrity = 0;
        }
        else
        {
            currHullIntegrity = newHullIntegrity;
        }
        armorGauge.updateChunks(currHullIntegrity);

    }

    public bool isShielded()
    {
        return shield.getShieldActive();
    }

	public float getShieldCharge(){
		return shield.getCurrShieldCharge();
	}

	public void setShieldCharge(int setCharge){
		shield.setCurrShieldCharge(setCharge);
	}

	public int getBombCharge () {
		return bomb.getBombCharge ();
	}

	public void setBombCharge (int newCharge) {
		bomb.setBombCharge (newCharge);
	}

	public float getUseCharge () {
		return bomb.getUseCharge ();
	}

	public void setUseCharge (float newCharge) {
		bomb.setUseCharge (newCharge);
	}

    public int getMaxHullIntegrity()
    {
        return maxHullIntegrity;
    }

    public int getCurrHullIntegrity()
    {
        return currHullIntegrity;
    }

    public void restoreHullPoint()
    {
		if (maxHullIntegrity > currHullIntegrity) {
			currHullIntegrity++;
            armorGauge.updateChunks(currHullIntegrity);
            audio_effects.PlayOneShot(hullRestoreSound);
		}

    }

	private void fireBullets() {

        // make direction and velocity vectors local for easy addition of components
        Vector3 localForward = transform.InverseTransformDirection(transform.forward);
        Vector3 localVelocity = new Vector3(0, 0, transform.InverseTransformDirection(rb.velocity).z);
        //GameObject newShot = (GameObject)Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //Debug.Log(transform.InverseTransformDirection(rb.velocity));
        //Debug.Log(transform.InverseTransformDirection(transform.forward));
        // add constant forward velocity to newShot and add the player's forward velocity component
        //newShot.GetComponent<Rigidbody>().velocity = transform.TransformDirection(localForward * 40 + localVelocity);

        /*Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 realBulletVel = aimDirNorm * (float)bulletVel;
		Vector3 shipVel = GetComponent<Rigidbody> ().velocity;
		shipVel.Normalize ();

		if (aimDirNorm != -1 * shipVel) {
			realBulletVel += GetComponent<Rigidbody> ().velocity;
		}*/

        float vol = Random.Range(volLowRange, volHighRange);
        audio_bullet.PlayOneShot(playerBulletSound, vol);

        for(int i = 0; i < bulletSpawnLocations.Length; ++i)
        {
            GameObject shotObj = (GameObject)Instantiate(bulletPrefab, bulletSpawnLocations[i].position, bulletSpawnLocations[i].rotation);
            PlayerBullet newShot = (PlayerBullet)shotObj.GetComponent(typeof(PlayerBullet));
            newShot.setVars(bulletColor, transform.TransformDirection(localForward * (float) bulletVel + localVelocity));
        }

		/*GameObject bulletObj = (GameObject) Instantiate (bulletPrefab, frontOfShip + transform.forward + transform.right * 1.4f - transform.up * 2.1f, transform.localRotation);
		PlayerBullet newBullet = (PlayerBullet)bulletObj.GetComponent(typeof(PlayerBullet));
		newBullet.setVars (bulletColor, realBulletVel);

		bulletObj = (GameObject) Instantiate (bulletPrefab, frontOfShip + transform.forward - transform.right * 1.4f - transform.up * 2.1f, transform.localRotation);
		newBullet = (PlayerBullet)bulletObj.GetComponent(typeof(PlayerBullet));
		newBullet.setVars (bulletColor, realBulletVel);*/
	}

	private void FireBomb (int strength) {
		Vector3 forwardNorm = transform.forward;
		forwardNorm.Normalize ();
		GameObject newBomb = (GameObject)Instantiate (powerBomb, powerBombSpawner.transform.position, Quaternion.identity);
		newBomb.GetComponent<PowerBomb> ().CalculateVariables (strength, forwardNorm);
		setBombCharge (getBombCharge () - strength);
		setUseCharge (0f);
		useGaugeIncreaseRate = 10f;
	}

    public int getPowerupIndex()
    {
        int index = 0;
        for(int i = 0; i < powerUpList.Length; ++i)
        {
            if(curPowerUp.CompareTo(powerUpList[i]) == 0)
            {
                index = i;
                break;
            }
        }
        return index;

    }

	public void EquipPowerUp (int numPowerUp) {
		curPowerUp = powerUpList [numPowerUp];
		if (curPowerUp.CompareTo ("") != 0) {
			PowerUp[] components = gameObject.GetComponents<PowerUp> ();
			switch (numPowerUp) {
			case 1:
				for (int numComp = 0; numComp < components.Length; ++numComp) {
					components [numComp].enabled = false;
				}
				break;
			case 2:
				for (int numComp = 0; numComp < components.Length; ++numComp) {
					components [numComp].enabled = false;
				}
				break;
			case 3:
				for (int numComp = 0; numComp < components.Length; ++numComp) {
					components [numComp].enabled = false;
				}
				break;
			default:
				curPowerUp = "";
				break;
			}
		}
	}

	public bool GetIsSlowed () {
		return isSlowed;
	}

	public void SlowTime (float timeScale) {
		isSlowed = true;
		gameObject.GetComponent<Rigidbody> ().mass /= timeScale;
		gameObject.GetComponent<Rigidbody> ().velocity *= timeScale;
		moveSpeed *= timeScale;
		rotSpeed *= timeScale;
		rollSpeed *= timeScale;
		bulletVel *= timeScale;
		fireRate /= timeScale;
		invulnSecs /= timeScale;
	}

	public void QuickTime (float timeScale) {
		isSlowed = false;
		gameObject.GetComponent<Rigidbody> ().mass *= timeScale;
		moveSpeed /= timeScale;
		rotSpeed /= timeScale;
		rollSpeed /= timeScale;
		bulletVel /= timeScale;
		fireRate *= timeScale;
		invulnSecs *= timeScale;
	}

	private void TurnOnThruster (string name) {
		switch (name) {
		case "main":
			mainThrusterLeft.startSpeed = 0f;
			mainThrusterRight.startSpeed = 0f;
			mainThrusterLeft.simulationSpace = ParticleSystemSimulationSpace.World;
			mainThrusterRight.simulationSpace = ParticleSystemSimulationSpace.World;

			if (trailLeft == null) {
				trailLeft = (GameObject) Instantiate (contrail);
				trailRight = (GameObject) Instantiate (contrail);
				trailLeft.GetComponent<DestroyContrail> ().GiveParent (transform);
				trailRight.GetComponent<DestroyContrail> ().GiveParent (transform);

				trailLeft.transform.localPosition = trailPositionLeft;
				trailRight.transform.localPosition = trailPositionRight;
			}
			break;
		default:
			break;
		}
	}

	private void TurnOffThruster (string name) {
		switch (name) {
		case "main":
			mainThrusterLeft.startSpeed = 1;
			mainThrusterRight.startSpeed = 1;
			mainThrusterLeft.simulationSpace = ParticleSystemSimulationSpace.Local;
			mainThrusterRight.simulationSpace = ParticleSystemSimulationSpace.Local;

			if (trailLeft != null) {
				trailLeft.transform.parent = null;
				trailRight.transform.parent = null;
				trailLeft = null;
				trailRight = null;
			}
			break;
		default:
			break;
		}
	}

    public void freezeRotation()
    {
        rb.freezeRotation = true;
    }

    public void unFreezeRotation()
    {
        rb.freezeRotation = false;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.transform.CompareTag("Environment"))
        {
            
            if (wallScrapeTime < 0.5f)
            {
                wallScrapeTime -= Time.deltaTime;
                if (wallScrapeTime <= 0)
                {
                    wallScrapeTime = 0.5f;
                }
                
            }
            if (wallScrapeTime >= 0.5f)
            {
                audio_wallImpact.PlayOneShot(ScrapeHullSound);
            }
            
        }
    }
}
