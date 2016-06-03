using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReactorCoreUIController : MonoBehaviour {

    private Canvas coreHPCanvas;
    private Image coreHPDisplay;
    private GameObject player;

    private HUDColorController colorController;
    public BossManager reactorShields;
    public reactorBomb reactorHP;

	// Use this for initialization
	void Start () {

        coreHPCanvas = GetComponentInChildren<Canvas>();
        coreHPDisplay = GetComponentInChildren<Image>();
        player = GameObject.FindGameObjectWithTag("Player");

        coreHPCanvas.enabled = false;

        colorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        coreHPDisplay.color = colorController.getColorByString("bomb");
	}
	
	// Update is called once per frame
	void Update () {
        if (reactorShields.isVulnerable() && !reactorHP.isDead())
        {
            coreHPCanvas.enabled = true;
            transform.LookAt(player.transform);
        }
	}

    public void colorUpdate()
    {
        coreHPDisplay.color = colorController.getColorByString("bomb");
    }
}
