using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathNotificationController : MonoBehaviour {

    private PlayerController player;
    private Image background;
    private Text deathText;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        background = GetComponent<Image>();
        deathText = gameObject.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.isDead())
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
