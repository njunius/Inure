using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 20.0f;
    public float rotSpeed = 120.0f;
    public float rollSpeed = 100.0f;

    // public Camera camera;
    public bool paused = false;


    private Rigidbody rb;
    private GameObject pauseCanvas;
    private Canvas pauseScreen;

    private ShieldController shield;

    public GameObject gameController;
    public InputManager im;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        pauseCanvas = GameObject.Find("Canvas");
        pauseScreen = pauseCanvas.GetComponent<Canvas>();
        pauseScreen.enabled = false;

        shield = GetComponentInChildren<ShieldController>();

        if (GameObject.Find("GameController") == null)
        {
            Instantiate(gameController);         
        }
        gameController = GameObject.FindGameObjectWithTag("GameController");
        im = gameController.GetComponent<InputManager>();

    }
	
	// Update is called once per frame
	void Update () {

        //Tongles pausing the game

        if (im.getInputDown("Pause") && !paused)
        {
            //Debug.Log("Pause!");
            paused = !paused;
            Time.timeScale = 0;
            pauseScreen.enabled = true;
        }
        else if (im.getInputDown("Pause") && paused)
        {
            //Debug.Log("UnPause!");
            paused = !paused;
            Time.timeScale = 1;
            pauseScreen.enabled = false;
        }

        // Shield Controls 
        if (im.getInputDown("Shield") && !shield.getShieldActive() && shield.isShieldCharged())
        {
            shield.setShieldActive(true);
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



    }

    public bool isShielded()
    {
        return shield.getShieldActive();
    }
}
