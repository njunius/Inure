using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathNotificationController : MonoBehaviour {

    private PlayerController player;
    private GameObject[] pauseItems;
    private GameObject[] deathItems;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        pauseItems = GameObject.FindGameObjectsWithTag("Pause Screen");
        deathItems = GameObject.FindGameObjectsWithTag("Death Screen");

	}
	
	// Update is called once per frame
	void Update () {
        if (player.isDead() && !player.tutorialMode)
        {
            for(int i = 0; i < pauseItems.Length; ++i)
            {
                pauseItems[i].SetActive(false);
            }

            for(int i = 0; i < deathItems.Length; ++i)
            {
                deathItems[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < pauseItems.Length; ++i)
            {
                pauseItems[i].SetActive(true);
            }

            for (int i = 0; i < deathItems.Length; ++i)
            {
                deathItems[i].SetActive(false);
            }
        }
	}
}
