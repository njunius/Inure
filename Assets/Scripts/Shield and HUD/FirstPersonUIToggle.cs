using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstPersonUIToggle : MonoBehaviour {

    public CameraController camera;
    private Canvas[] firstPersonHUD;
	private Renderer radar3D;
	public GameObject[] blips;
    private LineRenderer laserSight;

	// Use this for initialization
	void Start () {
        camera = Camera.main.GetComponent<CameraController>();
        firstPersonHUD = GetComponentsInChildren<Canvas>();
		radar3D = GameObject.FindGameObjectWithTag("Radar3D").GetComponent<Renderer>();
		blips = GameObject.FindGameObjectsWithTag("Blip");
        laserSight = GameObject.FindGameObjectWithTag("Laser Sight").GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update () {
		blips = GameObject.FindGameObjectsWithTag ("Blip");

	    if(camera.getMode() == CameraMode.ThirdPerson)
        {
			radar3D.enabled = false;
            laserSight.enabled = true;

			for (int i = 0; i < blips.Length; ++i) 
			{
				blips [i].GetComponent<Renderer>().enabled = false;
			}

            for(int i = 0; i < firstPersonHUD.Length; ++i)
            {
                firstPersonHUD[i].enabled = false;
            }
        }
        else
        {
			radar3D.enabled = true;
            laserSight.enabled = false;

			for (int i = 0; i < blips.Length; ++i) 
			{
				blips [i].GetComponent<Renderer>().enabled = true;
			}

            for (int i = 0; i < firstPersonHUD.Length; ++i)
            {
                firstPersonHUD[i].enabled = true;
            }
        }
	}
}
