using UnityEngine;
using System.Collections;

public class SettingsScreenOverlayControl : MonoBehaviour {

    public Canvas keybindingFields;
    private Canvas thisOverlay;

	// Use this for initialization
	void Start () {
        thisOverlay = GetComponent<Canvas>();
        thisOverlay.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (thisOverlay.enabled)
        {
            keybindingFields.enabled = true;
        }
        else
        {
            keybindingFields.enabled = false;
        }
	}
}
