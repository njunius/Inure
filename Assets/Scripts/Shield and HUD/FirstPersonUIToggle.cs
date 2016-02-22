using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstPersonUIToggle : MonoBehaviour {

    public CameraController camera;
    private Canvas[] firstPersonHUD;
	private Renderer radar3D;
	public GameObject[] blips;

	// Use this for initialization
	void Start () {
        camera = Camera.main.GetComponent<CameraController>();
        firstPersonHUD = GetComponentsInChildren<Canvas>();
		radar3D = GameObject.FindGameObjectWithTag("Radar3D").GetComponent<Renderer>();
		blips = GameObject.FindGameObjectsWithTag("Blip");

    }

    // Update is called once per frame
    void Update () {
		blips = GameObject.FindGameObjectsWithTag ("Blip");

	    if(camera.getMode() == CameraMode.ThirdPerson)
        {
			radar3D.enabled = false;

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
