﻿/*
 * Controls the player's physical movement
 * Responsible for interpreting the input manager into player actions during gameplay
 * Responsible for keeping track of player's hull integrity
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 20.0f;
    public float rotSpeed = 120.0f;
    public float rollSpeed = 100.0f;
    public Color bulletColor = Color.blue;
	public float bulletVel = 40.0f;
	public float fireRate = 0.2f;
	public GameObject bulletPrefab;

	public float invulnSecs = 1.0f;
	public bool noGameOver = false;

	private Vector3 frontOfShip;
	private bool isFiring = false;
	private bool isSlowed = false;

    public bool paused = false;

    // player armor/health stats
    private int maxHullIntegrity;
    private int currHullIntegrity;
	private bool fInvincible = false;
	private string[] powerUpList = new string[]{"", "PowerUp_EMP", "PowerUp_Shockwave", "PowerUp_SlowTime"};
	private string curPowerUp;

    private Rigidbody rb;
	private GameObject canvasOBJ, gameOverOBJ, pauseTxtOBJ, inureTxtOBJ; //UI GameObjects
    private Canvas UICanvas; //Base user interface, pause menu here
    private Canvas settingsOverlay;
	private RawImage gameOver; //Game Over IMG
	private Text pauseTxt, inureTxt;

	private float timerTMP = 0;
    
	private ShieldController shield;

    public GameObject gameController;
    public InputManager im;

    // Use this for initialization
    void Awake () {

        Time.timeScale = 1; // The time scale must be reset upon loading from the main menu

        rb = GetComponent<Rigidbody>();

		curPowerUp = powerUpList[0];

        settingsOverlay = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();

		//Set the UI objects and assign components 
		//(Wall of text to be fixed in future updates)
		canvasOBJ = GameObject.Find("Canvas");
        UICanvas = canvasOBJ.GetComponent<Canvas>();
		pauseTxtOBJ = GameObject.Find ("PausedTXT");
		pauseTxt = pauseTxtOBJ.GetComponent<Text> ();
		inureTxtOBJ = GameObject.Find ("InureTXT");
		inureTxt = inureTxtOBJ.GetComponent<Text> ();
		gameOverOBJ = GameObject.Find("GameOverIMG");
		gameOver = gameOverOBJ.GetComponent<RawImage> ();

		UICanvas.enabled = false;
		gameOver.enabled = false;

        shield = GetComponentInChildren<ShieldController>();

        maxHullIntegrity = currHullIntegrity = 5;

        if (GameObject.Find("GameController") == null)
        {
            Instantiate(gameController);         
        }
        gameController = GameObject.FindGameObjectWithTag("GameController");
        im = gameController.GetComponent<InputManager>();

    }
	
	// Update is called once per frame
	void Update () {

		//find new point at front of ship for firing
		Vector3 forwardNorm = gameObject.transform.forward;
		forwardNorm.Normalize ();
		frontOfShip = gameObject.GetComponent<Renderer> ().bounds.center + (forwardNorm * gameObject.GetComponent<Renderer> ().bounds.extents.z);

        //Toggles pausing the game

        if (im.getInputDown("Pause") && !paused)
        {
            paused = !paused;
            Time.timeScale = 0;
            UICanvas.enabled = true;
        }
        else if (im.getInputDown("Pause") && paused)
        {
            if (!settingsOverlay.enabled)
            {
                paused = !paused;
                Time.timeScale = 1;
                UICanvas.enabled = false;
            }
            else if (settingsOverlay.enabled)
            {
                settingsOverlay.enabled = false;
            }
        }

        // Shield Controls 
        if (im.getInput("Shield") > 0.3f && !shield.getShieldActive() && shield.isShieldCharged())
        {
            shield.setShieldActive(true);
        }

		// Shooting controls
		if (im.getInputDown("Shoot") && !isFiring)
		{
			isFiring = true;
			InvokeRepeating ("fireBullets", 0.0f, fireRate);
		}

		if (im.getInputUp ("Shoot"))
		{
			CancelInvoke ("fireBullets");
			isFiring = false;
		}

		if (im.getInputDown("Use Powerup") && curPowerUp != "")
		{
			PowerUp whichPowerUp = null;
			switch (curPowerUp) {
			case "PowerUp_EMP":
				GetComponent<PowerUp_EMP> ().enabled = true;
				GetComponent<PowerUp_EMP> ().Activate ();
				whichPowerUp = GetComponent<PowerUp_EMP> ();
				break;
			case "PowerUp_Shockwave":
				GetComponent<PowerUp_Shockwave> ().enabled = true;
				GetComponent<PowerUp_Shockwave> ().Activate ();
				whichPowerUp = GetComponent<PowerUp_Shockwave> ();
				break;
			case "PowerUp_SlowTime":
				GetComponent<PowerUp_SlowTime> ().enabled = true;
				GetComponent<PowerUp_SlowTime> ().Activate ();
				whichPowerUp = GetComponent<PowerUp_SlowTime> ();
				break;
			default:
				Debug.Log("Invalid powerup value");
				break;
			}

			if (whichPowerUp != null) {
				//whichPowerUp.enabled = false;
			}

			//gameObject.GetComponent<PowerUp> ().Activate ();
			//Destroy(gameObject.GetComponent<PowerUp> ());
			curPowerUp = "";
		}

		//Count down invulnerability
		if(fInvincible)
		{
			timerTMP -= Time.deltaTime;
			Debug.Log("TimerTMP = " + timerTMP);
		}

		if(timerTMP <= 0)
		{
			fInvincible = false;
		}
    }

    void FixedUpdate()
    {


        float moveLongitudinal = im.getInput("Longitudinal") * moveSpeed;
        float moveLateral = im.getInput("Lateral") * moveSpeed;
        float moveVertical = im.getInput("Vertical") * moveSpeed;
        float rotRoll = im.getInput("Roll") * rollSpeed;
        float rotPitch = im.getInput("Pitch") * rotSpeed;
        float rotYaw = im.getInput("Yaw") * rotSpeed;

        if (moveLongitudinal != 0)
        {
            if (Mathf.Sign(transform.InverseTransformDirection(rb.velocity).z) != Mathf.Sign(moveLongitudinal))
            {
                rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x,
                                                               transform.InverseTransformDirection(rb.velocity).y, 0));
            }
            rb.AddForce(transform.TransformDirection(Vector3.forward * moveLongitudinal));
        }
        else
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
        else
        {
            rb.velocity = transform.TransformDirection(new Vector3(0, transform.InverseTransformDirection(rb.velocity).y,
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
        else
        {
            rb.velocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.velocity).x, 0,
                                                               transform.InverseTransformDirection(rb.velocity).z));
        }


        /*if (moveLongitudinal == 0 && moveLateral == 0 && moveVertical == 0)
        {
            rb.velocity = Vector3.zero;
        }*/

        /*if (moveLateral != 0 || moveVertical != 0)
        {
            rb.AddForce(transform.TransformDirection(Vector3.forward * moveLongitudinal
                + Vector3.right * moveLateral + Vector3.up * moveVertical));
            //rb.velocity = transform.TransformDirection(Vector3.forward * moveLongitudinal 
                //+ Vector3.right * moveLateral + Vector3.up * moveVertical);
        }
        else
        {

            rb.velocity = Vector3.zero;
        }*/
        if (rotPitch != 0)
        {
            rb.AddTorque(transform.TransformDirection(Vector3.left * rotPitch));
        }
        else
        {
            rb.angularVelocity = transform.TransformDirection(new Vector3(0, transform.InverseTransformDirection(rb.angularVelocity).y,
                                                                    transform.InverseTransformDirection(rb.angularVelocity).z));
            
        }

        if (rotYaw != 0)
        {
            rb.AddTorque(transform.TransformDirection(Vector3.up * rotYaw));
        }
        else
        {
            rb.angularVelocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x, 0,
                                                               transform.InverseTransformDirection(rb.angularVelocity).z));
        }

        if (rotRoll != 0)
        {
            rb.AddTorque(transform.TransformDirection(Vector3.forward * rotRoll));
        }
        else
        {
            rb.angularVelocity = transform.TransformDirection(new Vector3(transform.InverseTransformDirection(rb.angularVelocity).x,
                                                               transform.InverseTransformDirection(rb.angularVelocity).y, 0));
            
        }
        if (rotPitch != 0 || rotYaw != 0 || rotRoll != 0)
        {
            //rb.AddTorque(transform.TransformDirection(Vector3.left * rotPitch + Vector3.up * rotYaw + Vector3.forward * rotRoll));
            rb.angularVelocity = transform.TransformDirection(Vector3.left * rotPitch + Vector3.up * rotYaw + Vector3.forward * rotRoll);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }


        if (rb.velocity.magnitude > 20.0f)
        {
            rb.velocity = rb.velocity.normalized * 20.0f;
        }

		//Activate the game over sequence when death is true
		if (isDead () && !noGameOver) 
		{
			killPlayer();
		}

    }

	//Transports the player to the specified coordinates
	//Resets their stats to saved data
	//Usually called by a button in the Canvas UI
	public void reloadCheckP (LastCheckpoint savedData)
	{
		Debug.Log("Reloading!");
		//Teleport Player + Camera
		gameObject.transform.position = savedData.getCheckPOS();
		gameObject.transform.rotation = savedData.getCheckROT();

		GameObject.FindGameObjectWithTag("MainCamera").transform.position = savedData.getCheckCamPOS();
		GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = savedData.getCheckCamROT();

		//Reset stats
		currHullIntegrity = savedData.getHealth();
		shield.setCurrShieldCharge(savedData.getShield());
		GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>().currBombCharge = savedData.getBomb();

		//Overwrite data
		savePlayer ();

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

	//Deactivates player controls and shows game over screen
	private void killPlayer()
	{
		//Show Game Over Screen
		pauseTxt.enabled = false;
		inureTxt.enabled = false;
		Time.timeScale = 0.3f;
		gameOver.enabled = true;
		UICanvas.enabled = true;
		//Destroy player
		this.enabled = false;
	}

	//Disable game over screen
	public void savePlayer ()
	{
		//Show Pause Screen
		pauseTxt.enabled = true;
		inureTxt.enabled = true;
		Time.timeScale = 1.0f;
		gameOver.enabled = false;
		UICanvas.enabled = false;
		//save player
		this.enabled = true;
	}

    // returns true if the player's hull integrity has dropped to 0
    public bool isDead()
    {
        return currHullIntegrity == 0;
    }

    /* 
     * reduces the player's hull integrity by 1
     * NOTE: the player's health is the number of hits that can be taken
     */
	public void takeDamage()
    {
        if (!fInvincible)
        {
            --currHullIntegrity;

            //become fInvincible for invulnSecs
            timerTMP = invulnSecs;
            fInvincible = true;
        }

        if (currHullIntegrity < 0)
        {
            currHullIntegrity = 0;
        }

    }

    /*
     * newHullIntegrity cannot be greater than maxHullIntegrity
     * newHullIntegrity cannot be less than 0
     */
    public void setHullIntegrity(int newHullIntegrity)
    {
        currHullIntegrity = newHullIntegrity;
    }

    public bool isShielded()
    {
        return shield.getShieldActive();
    }

	public int getShieldCharge(){
		return shield.getCurrShieldCharge();
	}

	public void setShieldCharge(int setCharge){
		shield.setCurrShieldCharge(setCharge);
	}

    public int getMaxHullIntegrity()
    {
        return maxHullIntegrity;
    }

    public int getCurrHullIntegrity()
    {
        return currHullIntegrity;
    }

	private void fireBullets() {
		Vector3 aimDirNorm = gameObject.transform.forward;
		aimDirNorm.Normalize ();
		Vector3 realBulletVel = aimDirNorm * (float)bulletVel;
		Vector3 shipVel = GetComponent<Rigidbody> ().velocity;
		shipVel.Normalize ();

		if (aimDirNorm != -1 * shipVel) {
			realBulletVel += GetComponent<Rigidbody> ().velocity;
		}

		GameObject bulletObj = (GameObject) Instantiate (bulletPrefab, frontOfShip + transform.forward + transform.right, transform.localRotation);
		PlayerBullet newBullet = (PlayerBullet)bulletObj.GetComponent(typeof(PlayerBullet));
		newBullet.setVars (bulletColor, realBulletVel);

		bulletObj = (GameObject) Instantiate (bulletPrefab, frontOfShip + transform.forward - transform.right, transform.localRotation);
		newBullet = (PlayerBullet)bulletObj.GetComponent(typeof(PlayerBullet));
		newBullet.setVars (bulletColor, realBulletVel);
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
				Debug.Log ("New PowerUp is null");
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
}
