using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialEventManager : MonoBehaviour {
    public PlayerController player;
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

    private GameObject gameController;
    private InputManager im;

    private AudioSource source;
    public AudioClip Initialize;
    public AudioClip Vertical;
    public AudioClip Accelerators;
    public AudioClip Rotators;
    public AudioClip Weapons;

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
                player.sheildEnabled = true;
                player.transform.position = DebugTutorialLocations[1].transform.position;
                player.transform.rotation = DebugTutorialLocations[1].transform.rotation;
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = DebugTutorialLocations[1].transform.position;
                while (player.getCurrHullIntegrity() < 5)
                {
                    player.restoreHullPoint();
                }
                player.tutorialMode = false;
                eventIndex = 15;
            }
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
            player.sheildEnabled = false;
        }
	}


    public void refresh()
    {
        Debug.Log("Event " + eventIndex);
        switch (eventIndex)
        {
            case 0:         //Scene ends with hull indicatiors activated.

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
                subtitles.text = "Press " + im.getPosInputName("Vertical") + " or " + im.getNegInputName("Vertical") + " to move Vertically.";
                player.verticalEnginesEnabled = true;
                
                break;

            case 7:         //Doors open
                subtitles.text = "";
                
                door1Lower.GetComponent<TutorialDoors>().activate();
                door1Upper.GetComponent<TutorialDoors>().activate();
                door2Lower.GetComponent<TutorialDoors>().activate();
                door2Upper.GetComponent<TutorialDoors>().activate();
                break;
            case 8:         //Player can lift off. Display vertical controls
                source.PlayOneShot(Accelerators);
                subtitles.text = "Press " + im.getPosInputName("Longitudinal").ToUpper() + im.getNegInputName("Longitudinal").ToUpper() + im.getNegInputName("Lateral").ToUpper() + im.getPosInputName("Lateral").ToUpper() + " to accelerate in Longitudinal or Lateral.";
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                break;

            case 9:
                subtitles.text = "";
                break;

            case 10:         //Player can move forward.
                source.PlayOneShot(Rotators);
                subtitles.text = "Use Mouse to rotate Pitch and Yaw.  Use " + im.getPosInputName("Roll").ToUpper() + " and " + im.getNegInputName("Roll").ToUpper() + " to Roll.";
                player.rotateEnabled = true;
                player.unFreezeRotation();
                break;
            case 11:
                subtitles.text = "";
                subtitles.enabled = false;
                break;
            case 12:
                subtitles.enabled = true;
                source.PlayOneShot(Weapons);
                subtitles.text = "Use Left Mouse Button to fire.";
                player.weaponsEnabled = true;
                player.tutorialMode = false;


                break;
            case 13:
                subtitles.text = "";
                subtitles.enabled = false;
                interior.SetActive(false);
                exterior.SetActive(true);
                break;
            case 14:
                //source.PlayOneShot(Shields);
                subtitles.enabled = true;
                subtitles.text = "Use Right Mouse Button to activate shield.";
                player.sheildEnabled = true;
                break;
            case 15:
                subtitles.text = "";
                subtitles.enabled = false;
                player.targetLocked = false;
                player.unFreezeRotation();
                player.rotateEnabled = true;
                player.verticalEnginesEnabled = true;
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                player.weaponsEnabled = true;
                player.sheildEnabled = true;
                break;

        }
    }
}
