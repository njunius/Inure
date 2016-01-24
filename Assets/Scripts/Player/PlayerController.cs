/*
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

    public bool paused = false;

    // player armor/health stats
    private int maxHullIntegrity;
    public int currHullIntegrity; //Changed to Public for outside scripting
    private Image armorGauge;

    private Rigidbody rb;
	private GameObject canvasOBJ, gameOverOBJ, pauseTxtOBJ, inureTxtOBJ; //UI GameObjects
    private Canvas UICanvas; //Base user interface, pause menu here
	private RawImage gameOver; //Game Over IMG
	private Text pauseTxt, inureTxt;

    private ShieldController shield;

    public GameObject gameController;
    public InputManager im;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        
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
        armorGauge = GameObject.FindGameObjectWithTag("Armor Gauge").GetComponent<Image>();

        maxHullIntegrity = currHullIntegrity = 100;

        if (GameObject.Find("GameController") == null)
        {
            Instantiate(gameController);         
        }
        gameController = GameObject.FindGameObjectWithTag("GameController");
        im = gameController.GetComponent<InputManager>();

    }
	
	// Update is called once per frame
	void Update () {

        //Toggles pausing the game

        if (im.getInputDown("Pause") && !paused)
        {
            paused = !paused;
            Time.timeScale = 0;
            UICanvas.enabled = true;
        }
        else if (im.getInputDown("Pause") && paused)
        {
            //Debug.Log("UnPause!");
            paused = !paused;
            Time.timeScale = 1;
            UICanvas.enabled = false;
        }

        // Shield Controls 
        if (im.getInput("Shield") > 0.3f && !shield.getShieldActive() && shield.isShieldCharged())
        {
            shield.setShieldActive(true);
        }

        // Armor Gauge control
        armorGauge.fillAmount = (float)currHullIntegrity / (float)maxHullIntegrity;
    }

    void FixedUpdate()
    {


        float moveLongitudinal = im.getInput("Longitudinal") * moveSpeed;
        float moveLateral = im.getInput("Lateral") * moveSpeed;
        float moveVertical = im.getInput("Vertical") * moveSpeed;
        float rotRoll = im.getInput("Roll") * rollSpeed;
        float rotPitch = im.getInput("Pitch") * rotSpeed;
        float rotYaw = im.getInput("Yaw") * rotSpeed;


        if (moveLongitudinal != 0 || moveLateral != 0 || moveVertical != 0)
        {
            rb.velocity = transform.TransformDirection(Vector3.forward * moveLongitudinal 
                + Vector3.right * moveLateral + Vector3.up * moveVertical);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if (rotPitch != 0 || rotYaw != 0 || rotRoll != 0)
        {
            rb.angularVelocity = transform.TransformDirection(Vector3.left * rotPitch + Vector3.up * rotYaw + Vector3.forward * rotRoll);
        }

		//Activate the game over sequence when death is true
		if (isDead ()) 
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

    }

    // returns true if the player's hull integrity has dropped to 0
    public bool isDead()
    {
        return currHullIntegrity == 0;
    }

    /* pre: damage is a positive number
     * reduces the player's hull integrity by a specified amount
     * NOTE: the player's health is base 100
     */
    public void takeDamage(int damage)
    {
        currHullIntegrity -= damage;

        if(currHullIntegrity < 0)
        {
            currHullIntegrity = 0;
        }
    }

    // returns the status of the player's shield
    public bool isShielded()
    {
        return shield.getShieldActive();
    }
}
