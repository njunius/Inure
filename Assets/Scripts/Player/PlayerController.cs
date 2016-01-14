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


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        pauseCanvas = GameObject.Find("Canvas");
        pauseScreen = pauseCanvas.GetComponent<Canvas>();
        pauseScreen.enabled = false;

        shield = GetComponentInChildren<ShieldController>();
    }
	
	// Update is called once per frame
	void Update () {
        
        //Tongles pausing the game
        if (Input.GetButtonDown("Pause") && !paused)
        {
            //Debug.Log("Pause!");
            paused = !paused;
            Time.timeScale = 0;
            pauseScreen.enabled = true;
        }
        else if (Input.GetButtonDown("Pause") && paused)
        {
            //Debug.Log("UnPause!");
            paused = !paused;
            Time.timeScale = 1;
            pauseScreen.enabled = false;
        }

        // Shield Controls 
        if (Input.GetButtonDown("Shield") && !shield.getShieldActive() && shield.isShieldCharged())
        {
            shield.setShieldActive(true);
        }

        for (int i = 0; i < 20; i++)
        {
            if (Input.GetAxis("joystick button " + i) > 0)
            {
                Debug.Log("Button " + i + " was pressed!");
            }
        }
    }

    void FixedUpdate()
    {
        

        float moveLongitudinal = Input.GetAxis("Longitudinal") * moveSpeed;
        float moveLateral = Input.GetAxis("Lateral") * moveSpeed;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed;
        float rotPitch = Input.GetAxis("Pitch") * rotSpeed * Time.deltaTime;
        float rotYaw = Input.GetAxis("Yaw") * rotSpeed * Time.deltaTime;
        float rotRoll = Input.GetAxis("Roll") * rollSpeed * Time.deltaTime;


        if (moveLongitudinal != 0 || moveLateral != 0 || moveVertical != 0)
        {
            rb.velocity = transform.TransformDirection(Vector3.forward * moveLongitudinal + Vector3.right * moveLateral + Vector3.up * moveVertical);
            /*rb.transform.Translate(Vector3.forward * moveLongitudinal);
            rb.transform.Translate(Vector3.right * moveLateral);
            rb.transform.Translate(Vector3.up * moveVertical);*/
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
        rb.transform.Rotate(Vector3.left * rotPitch);
        rb.transform.Rotate(Vector3.up * rotYaw);
        rb.transform.Rotate(Vector3.forward * rotRoll);

        /*if (camera != null)
        {
            camera.transform.rotation = transform.rotation;
            camera.transform.position = transform.position;
            camera.transform.Translate(Vector3.back * 3);
        } Camera following is now in its own script. Will delete once successfully implemented -Nick */

        
    }

    public bool isShielded()
    {
        return shield.getShieldActive();
    }
}
