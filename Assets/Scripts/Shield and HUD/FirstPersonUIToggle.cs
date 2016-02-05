using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstPersonUIToggle : MonoBehaviour {

    public CameraController camera;
    private Canvas[] firstPersonHUD;

	// Use this for initialization
	void Start () {
        camera = Camera.main.GetComponent<CameraController>();
        firstPersonHUD = GetComponentsInChildren<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(camera.getMode() == CameraMode.ThirdPerson)
        {
            for(int i = 0; i < firstPersonHUD.Length; ++i)
            {
                firstPersonHUD[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < firstPersonHUD.Length; ++i)
            {
                firstPersonHUD[i].enabled = true;
            }
        }
	}
}
