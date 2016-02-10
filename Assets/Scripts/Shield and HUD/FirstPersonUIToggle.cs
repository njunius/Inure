using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstPersonUIToggle : MonoBehaviour {

    public CameraController camera;
    private Canvas[] firstPersonHUD;
    private GameObject Radar3D;

	// Use this for initialization
	void Start () {
        camera = Camera.main.GetComponent<CameraController>();
        firstPersonHUD = GetComponentsInChildren<Canvas>();
        Radar3D = GameObject.FindGameObjectWithTag("Radar3D");

    }

    // Update is called once per frame
    void Update () {
	    if(camera.getMode() == CameraMode.ThirdPerson)
        {
            Radar3D.SetActive(false); ;
            for(int i = 0; i < firstPersonHUD.Length; ++i)
            {
                firstPersonHUD[i].enabled = false;
            }
        }
        else
        {
            Radar3D.SetActive(true);
            for (int i = 0; i < firstPersonHUD.Length; ++i)
            {
                firstPersonHUD[i].enabled = true;
            }
        }
	}
}
