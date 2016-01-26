using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThreatQuadrantController : MonoBehaviour {

    public ThreatTriggerController trackedQuadrant;
    private Image quadrantIndicator;
    private float threatAlpha;
    private float bulletsToAlpha;

	// Use this for initialization
	void Start () {
        quadrantIndicator = GetComponent<Image>();
        threatAlpha = 0.0f;
        bulletsToAlpha = 0.01f; //value to be tweaked
	}
	
	// Update is called once per frame
	void Update () {
        threatAlpha = bulletsToAlpha * trackedQuadrant.getNumBullets();
	    quadrantIndicator.color = new Color(quadrantIndicator.color.r, quadrantIndicator.color.b, quadrantIndicator.color.g, threatAlpha);
	}
}
