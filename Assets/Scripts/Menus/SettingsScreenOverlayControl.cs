using UnityEngine;
using System.Collections;

public class SettingsScreenOverlayControl : MonoBehaviour {

    public GameObject keybindingFields;
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
            keybindingFields.SetActive(true);
        }
        else
        {
            keybindingFields.SetActive(false);
        }
	}
}
