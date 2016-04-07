using UnityEngine;
using System.Collections;

public class TutorialEventManager : MonoBehaviour {
    public PlayerController player;
    public int eventIndex = 0;
    private float eventTimer = 0;
    public GameObject door1Upper;
    public GameObject door1Lower;

	// Use this for initialization
	void Start () {
        eventTimer = 4;
        refresh();
	}
	
	// Update is called once per frame
	void Update () {
	    if (player != null)
        {
            if (eventTimer > 0)
            {
                eventTimer--;
                if (eventTimer == 0)
                {
                    eventIndex++;
                }
            }

            switch (eventIndex)
            {
                case 1:
                    refresh();
                    break;
                case 2:
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
            player.rotateEnabled = false;
            player.verticalEnginesEnabled = true;
            player.longitudinalEnginesEnabled = false;
            player.lateralEnginesEnabled = false;
            player.weaponsEnabled = false;
            player.sheildEnabled = false;
        }
	}


    public void refresh()
    {
        Debug.Log("Refresh");
        switch (eventIndex)
        {
            case 0:         //Scene ends with hull indicatiors activated.
                eventIndex++;
                
                Debug.Log("Event 0");
                break;
            case 1:
                player.verticalEnginesEnabled = true;
                Debug.Log("Event 1");
                break;

            case 2:         //Doors open
                door1Lower.GetComponent<TutorialDoors>().activate();
                door1Upper.GetComponent<TutorialDoors>().activate();
                Debug.Log("Event 2");
                break;
            case 3:         //Player can lift off. Display vertical controls
                player.longitudinalEnginesEnabled = true;
                player.lateralEnginesEnabled = true;
                Debug.Log("Event 3");
                break;
            case 4:         //Player can move forward.
                eventIndex = 3;
                player.rotateEnabled = true;
                Debug.Log("Event 4");
                break;
            case 5:         //Player can move lateral
            case 6:         //Player can rotate
            case 7:         //Player can shoot
            case 8:         //Player can sheild
            case 9:         //Player can proceed



                break;
        }
    }
}
