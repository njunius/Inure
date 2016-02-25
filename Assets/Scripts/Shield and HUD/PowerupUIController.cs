using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerupUIController : MonoBehaviour {

    public Sprite[] powerupSprites;
    private Image[] powerupGauges;
    private PlayerController player;

	// Use this for initialization
	void Start () {
        powerupSprites = new Sprite[] { Resources.Load<Sprite>("UI Sprites/EmptyPowerup"), Resources.Load<Sprite>("UI Sprites/EMPPlaceHolder"),
                                        Resources.Load<Sprite>("UI Sprites/ShockwavePlaceHolder"), Resources.Load<Sprite>("UI Sprites/TimeSlowPlaceholder")};


        GameObject[] temp = GameObject.FindGameObjectsWithTag("Powerup Gauge");
        powerupGauges = new Image[temp.Length];

        for (int i = 0; i < powerupGauges.Length; ++i)
        {
            powerupGauges[i] = temp[i].GetComponent<Image>();
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < powerupGauges.Length; ++i)
        {
            powerupGauges[i].sprite = powerupSprites[player.getPowerupIndex()];
        }
    }
}
