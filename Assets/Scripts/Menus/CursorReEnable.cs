using UnityEngine;
using System.Collections;

// Also will enable sound to play
public class CursorReEnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        AudioListener.pause = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }

        if (!AudioListener.pause)
        {
            AudioListener.pause = false;
        }
    }
}
