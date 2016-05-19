using UnityEngine;
using System.Collections;

public class SettingsExitBuffer : MonoBehaviour {

    private bool keyBindingSelected;

	// Use this for initialization
	void Awake () {
        keyBindingSelected = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool keyBindingIsSelected()
    {
        return keyBindingSelected;
    }

    public void setSelected(bool isSelected)
    {
        keyBindingSelected = isSelected;
    }
}
