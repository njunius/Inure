using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ResetKeybindings : MonoBehaviour, IPointerClickHandler {

    // keys and defaultKeys must be the same length
    public CustomKeyController[] keys;

    private string[] defaultKeys;

	// Use this for initialization
	void Awake () {

        // Values must be hard coded as the default never changes
        defaultKeys = new string[11];

        defaultKeys[0] = "w";
        defaultKeys[1] = "s";
        defaultKeys[2] = "d";
        defaultKeys[3] = "a";
        defaultKeys[4] = "space";
        defaultKeys[5] = "leftshift";
        defaultKeys[6] = "e";
        defaultKeys[7] = "q";
        defaultKeys[8] = "mouse0";
        defaultKeys[9] = "mouse1";
        defaultKeys[10] = "r";
    }

	public void OnPointerClick (PointerEventData eventData) {
        // keys and default keys need to be the same length
	    for(int i = 0; i < keys.Length; ++i)
        {
            keys[i].resetKey(defaultKeys[i]);
        }
	}
}
