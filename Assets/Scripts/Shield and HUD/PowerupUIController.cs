using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerupUIController : MonoBehaviour, HUDElement {

    public Sprite[] powerupSprites;
    private Image[] powerupGauges;
    private PlayerController player;

    private HUDColorController hudColorController;
    private string hudElementName;

    // Use this for initialization
    void Start () {

        hudElementName = "powerup";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        powerupSprites = new Sprite[] { Resources.Load<Sprite>("UI Sprites/EmptyPowerup"), Resources.Load<Sprite>("UI Sprites/EMPPlaceHolder"),
                                        Resources.Load<Sprite>("UI Sprites/ShockwavePlaceHolder"), Resources.Load<Sprite>("UI Sprites/TimeSlowPlaceholder")};


        GameObject[] temp = GameObject.FindGameObjectsWithTag("Powerup Gauge");
        powerupGauges = new Image[temp.Length];

        for (int i = 0; i < powerupGauges.Length; ++i)
        {
            powerupGauges[i] = temp[i].GetComponent<Image>();
            powerupGauges[i].color = hudColorController.getColorByString(hudElementName);
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

    public void UpdateColor()
    {
        for (int i = 0; i < powerupGauges.Length; ++i)
        {
            powerupGauges[i].color = hudColorController.getColorByString(hudElementName);
        }
    }
}
