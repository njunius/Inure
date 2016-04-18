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
                subtitles.text = "Press " + im.getPosInputName("Longitudinal").ToUpper() + " " + im.getNegInputName("Longitudinal").ToUpper() + " " + im.getPosInputName("Lateral").ToUpper() + " " + im.getNegInputName("Lateral").ToUpper() + " to accelerate in Longitudinal or Lateral.";
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                break;
            case 9:         //Player can move forward.
                source.PlayOneShot(Rotators);
                subtitles.text = "Use Mouse to rotate Pitch and Yaw.  Use " + im.getPosInputName("Roll").ToUpper() + " and " + im.getNegInputName("Roll").ToUpper() + " to Roll.";
                player.rotateEnabled = true;
                player.unFreezeRotation();
                break;
            case 10:
                subtitles.text = "";
                break;
            case 11:
                source.PlayOneShot(Weapons);
                subtitles.text = "Use Left Mouse Button to fire.";
                player.weaponsEnabled = true;



                break;
        }
    }
}
