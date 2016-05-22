using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialEventManager : MonoBehaviour {
    public PlayerController player;
    public ShieldController shield;
    public int eventIndex = 0;
    private float eventTimer = 0;
    public GameObject door1Upper;
    public GameObject door1Lower;
    public GameObject door2Upper;
    public GameObject door2Lower;

    public GameObject[] DebugTutorialLocations;

    public GameObject interior;
    public GameObject exterior;

    public Text subtitles;
    public Canvas subtitleCanvas;

    private GameObject gameController;
    private InputManager im;

    private AudioSource source;
    public AudioClip Initialize;
    public AudioClip Vertical;
    public AudioClip Longitudinal;
    public AudioClip Lateral;
    public AudioClip ManuveringOnline;
    public AudioClip Weapons;
    public AudioClip Course;
    public AudioClip ShieldCharging;
    public AudioClip ShieldReady;
    public AudioClip ManuveringEnabled;


    private float timer = 0;
    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        eventTimer = 150;
        subtitles.text = "";
        source.PlayOneShot(Initialize);

        
    }
	

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (eventIndex < 11)
            {
                player.unFreezeRotation();
                player.rotateEnabled = true;
                player.verticalEnginesEnabled = true;
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                player.weaponsEnabled = true;
                player.transform.position = DebugTutorialLocations[0].transform.position;
                player.transform.rotation = DebugTutorialLocations[0].transform.rotation;
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = DebugTutorialLocations[0].transform.position;
                while (player.getCurrHullIntegrity() < 5)
                {
                    player.restoreHullPoint();
                }
                player.tutorialMode = false;
                eventIndex = 11;

            }
            else if (eventIndex < 14)
            {
                interior.SetActive(false);
                exterior.SetActive(true);
                player.targetLocked = false;
                player.unFreezeRotation();
                player.rotateEnabled = true;
                player.verticalEnginesEnabled = true;
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                player.weaponsEnabled = true;
                player.shieldEnabled = true;
                shield.setShieldEnabled(true);
                player.transform.position = DebugTutorialLocations[1].transform.position;
                player.transform.rotation = DebugTutorialLocations[1].transform.rotation;
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = DebugTutorialLocations[1].transform.position;
                while (player.getCurrHullIntegrity() < 5)
                {
                    player.restoreHullPoint();
                }
                player.tutorialMode = false;
                eventIndex = 14;
            }
            else
            {
                interior.SetActive(false);
                exterior.SetActive(true);
                player.targetLocked = false;
                player.unFreezeRotation();
                player.rotateEnabled = true;
                player.verticalEnginesEnabled = true;
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                player.weaponsEnabled = true;
                player.shieldEnabled = true;
                shield.setShieldEnabled(true);
                player.transform.position = DebugTutorialLocations[2].transform.position;
                player.transform.rotation = DebugTutorialLocations[2].transform.rotation;
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = DebugTutorialLocations[2].transform.position;
                while (player.getCurrHullIntegrity() < 5)
                {
                    player.restoreHullPoint();
                }
                player.tutorialMode = false;
                eventIndex = 15;
            }
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            subtitles.text = "";
            subtitles.enabled = false;
            subtitleCanvas.enabled = false;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController");
            im = gameController.GetComponent<InputManager>();
        }
        


        if (player != null)
        {

            if (eventTimer > 0)
            {
                eventTimer--;
                if (eventTimer == 0)
                {
                    eventIndex++;
                    refresh();
                }
            }

            switch (eventIndex)
            {
                case 7:
                    if (door1Upper.GetComponent<TutorialDoors>().open)
                    {
                        eventIndex++;
                        refresh();
                    }
                    break;
            }


        }
        else if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.freezeRotation();
            player.rotateEnabled = false;
            player.verticalEnginesEnabled = false;
            player.longitudinalEnginesEnabled = false;
            player.lateralEnginesEnabled = false;
            player.weaponsEnabled = false;
            player.shieldEnabled = false;
            shield = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShieldController>();
            
        }
	}


    public void refresh()
    {
        //Debug.Log("Event " + eventIndex);
        switch (eventIndex)
        {
            case 0:         //Scene ends with hull indicatiors activated.
                subtitleCanvas.enabled = false;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                player.restoreHullPoint();
                eventTimer = 20;

                
                break;
            case 6:
                source.PlayOneShot(Vertical);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Press [" + im.getPosInputName("Vertical") + "] or [" + im.getNegInputName("Vertical") + "] to move Vertically.";
                player.verticalEnginesEnabled = true;
                
                break;

            case 7:         //Doors open
                subtitles.text = "";
                subtitles.enabled = false;
                subtitleCanvas.enabled = false;
                door1Lower.GetComponent<TutorialDoors>().activate();
                door1Upper.GetComponent<TutorialDoors>().activate();
                door2Lower.GetComponent<TutorialDoors>().activate();
                door2Upper.GetComponent<TutorialDoors>().activate();
                break;
            case 8:         //Player can lift off. Display vertical controls
                source.PlayOneShot(Longitudinal);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Press [" + im.getPosInputName("Longitudinal").ToUpper() + "] or [" + im.getNegInputName("Longitudinal").ToUpper() + "] to accelerate forward and back.";
                player.longitudinalEnginesEnabled = true;
                
                break;

            case 9:
                source.PlayOneShot(Lateral);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Press [" + im.getNegInputName("Lateral").ToUpper() + "] or [" + im.getPosInputName("Lateral").ToUpper() + "] to accelerate side to side.";
                player.lateralEnginesEnabled = true;
                break;

            case 10:         //Player can move forward.
                source.PlayOneShot(ManuveringOnline);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Use Mouse to rotate Pitch and Yaw.  Use [" + im.getPosInputName("Roll").ToUpper() + "] or [" + im.getNegInputName("Roll").ToUpper() + "] to Roll.";
                player.rotateEnabled = true;
                player.unFreezeRotation();
                break;
            case 11:
                subtitles.text = "";
                subtitles.enabled = false;
                subtitleCanvas.enabled = false;
                break;
            case 12:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                source.PlayOneShot(Weapons);
                subtitles.text = "Use Left Mouse Button to fire.";
                player.weaponsEnabled = true;
                player.tutorialMode = false;


                break;
            case 13:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                source.PlayOneShot(Course);
                subtitles.text = "Approach course confirmed.  Manuvering thrusters dissabled.";
                interior.SetActive(false);
                exterior.SetActive(true);
                break;
            case 14:
                source.PlayOneShot(ShieldCharging);
                /*subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Use Right Mouse Button to activate shield.";*/
                player.shieldEnabled = true;
                shield.setShieldEnabled(true);
                break;
            case 15:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                source.PlayOneShot(ManuveringEnabled);
                subtitles.text = "Manuvering thursters re-engaged.";
                player.targetLocked = false;
                player.unFreezeRotation();
                player.rotateEnabled = true;
                player.verticalEnginesEnabled = true;
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                player.weaponsEnabled = true;
                player.shieldEnabled = true;
                shield.setShieldEnabled(true);
                break;
            case 16:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                source.PlayOneShot(ShieldReady);
                subtitles.text = "Use [" + im.getPosInputName("Shield") + "] to activate shield and absorb enemy projectiles.";
                break;
            case 17:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                //source.PlayOneShot(ShieldReady);
                subtitles.text = "Hold [" + im.getPosInputName("Launch Bomb") + "] to charge bomb and release to launch.";
                break;
                

        }
    }
}
