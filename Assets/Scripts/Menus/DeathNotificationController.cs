using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathNotificationController : MonoBehaviour {

    private PlayerController player;
    private Image background;
    public Text deathText;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        background = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.isDead() && !player.tutorialMode)
        {
            background.enabled = true;
            deathText.enabled = true;
        }
        else
        {
            background.enabled = false;
            deathText.enabled = false;
        }
	}
}
