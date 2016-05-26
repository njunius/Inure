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

    private AudioSource computerSource;
    private AudioSource dialogueSource;
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


    public AudioClip dialogue1_a;
    public AudioClip dialogue1_b;
    public AudioClip dialogue2;
    public AudioClip dialogue3;
    public AudioClip dialogue4;
    public AudioClip dialogue5;


    private float timer = 0;
    // Use this for initialization
    void Start () {
        computerSource = GetComponents<AudioSource>()[0];
        dialogueSource = GetComponents<AudioSource>()[1];
        eventTimer = 500;
        subtitles.text = "";
        
        dialogueSource.PlayOneShot(dialogue1_a);

    }
	

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (eventIndex < 12)
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
                eventIndex = 12;
                eventTimer = 0;

            }
            else if (eventIndex < 16)
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
                eventIndex = 16;
                eventTimer = 0;
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
                eventIndex = 17;
                eventTimer = 0;
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
                case 0:
                    if (!dialogueSource.isPlaying)
                    {
                        eventIndex++;
                        refresh();
                    }
                    break;
                case 8:
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
                computerSource.PlayOneShot(Initialize);
                eventTimer = 150;
                break;
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                player.restoreHullPoint();
                eventTimer = 20;

                
                break;
            case 7:
                computerSource.PlayOneShot(Vertical);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Press [" + im.getPosInputName("Vertical") + "] or [" + im.getNegInputName("Vertical") + "] to move Vertically.";
                player.verticalEnginesEnabled = true;
                
                break;

            case 8:         //Doors open
                subtitles.text = "";
                subtitles.enabled = false;
                subtitleCanvas.enabled = false;
                door1Lower.GetComponent<TutorialDoors>().activate();
                door1Upper.GetComponent<TutorialDoors>().activate();
                door2Lower.GetComponent<TutorialDoors>().activate();
                door2Upper.GetComponent<TutorialDoors>().activate();
                break;
            case 9:         //Player can lift off. Display vertical controls
                computerSource.PlayOneShot(Longitudinal);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Press [" + im.getPosInputName("Longitudinal").ToUpper() + "] or [" + im.getNegInputName("Longitudinal").ToUpper() + "] to accelerate forward and back.";
                player.longitudinalEnginesEnabled = true;
                
                break;

            case 10:
                computerSource.PlayOneShot(Lateral);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Press [" + im.getNegInputName("Lateral").ToUpper() + "] or [" + im.getPosInputName("Lateral").ToUpper() + "] to accelerate side to side.";
                player.lateralEnginesEnabled = true;
                break;

            case 11:         //Player can move forward.
                computerSource.PlayOneShot(ManuveringOnline);
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Use Mouse to rotate Pitch and Yaw.  Use [" + im.getPosInputName("Roll").ToUpper() + "] or [" + im.getNegInputName("Roll").ToUpper() + "] to Roll.";
                player.rotateEnabled = true;
                player.unFreezeRotation();
                break;
            case 12:
                dialogueSource.Stop();
                dialogueSource.PlayOneShot(dialogue1_b);
                subtitles.text = "";
                subtitles.enabled = false;
                subtitleCanvas.enabled = false;
                break;
            case 13:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                computerSource.PlayOneShot(Weapons);
                dialogueSource.Stop();
                dialogueSource.PlayOneShot(dialogue2);
                subtitles.text = "Use [" + im.getPosInputName("Shoot").ToUpper() + "] to fire.";
                player.weaponsEnabled = true;
                player.tutorialMode = false;


                break;
            case 14:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                
                computerSource.PlayOneShot(Course);
                subtitles.text = "Approach course confirmed.  Manuvering thrusters dissabled.";
                interior.SetActive(false);
                exterior.SetActive(true);
                break;
            case 15:
                dialogueSource.Stop();
                dialogueSource.PlayOneShot(dialogue3);
                break;
            case 16:
                computerSource.PlayOneShot(ShieldCharging);
                /*subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                subtitles.text = "Use Right Mouse Button to activate shield.";*/
                player.shieldEnabled = true;
                shield.setShieldEnabled(true);
                break;
            case 17:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                computerSource.PlayOneShot(ManuveringEnabled);
                dialogueSource.Stop();
                dialogueSource.PlayOneShot(dialogue4);
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
            case 18:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                computerSource.PlayOneShot(ShieldReady);
                dialogueSource.Stop();
                dialogueSource.PlayOneShot(dialogue5);
                subtitles.text = "Use [" + im.getPosInputName("Shield").ToUpper() + "] to activate shield and absorb enemy projectiles.";
                break;
            case 19:
                subtitleCanvas.enabled = true;
                subtitles.enabled = true;
                timer = 10;
                //source.PlayOneShot(ShieldReady);
                subtitles.text = "Hold [" + im.getPosInputName("Launch Bomb").ToUpper() + "] to charge bomb and release to launch. The charge required is always displayed on the door.";
                break;
                

        }
    }
}
