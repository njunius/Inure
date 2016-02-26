using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CustomKeyController : MonoBehaviour {

    private InputManager inputs;
    private Dictionary<string, InputBinding> inputBindings;
    public Text currentKey;
    private string key;
    private bool initialized;

    public string command;
    public bool positiveDirection;

	// Use this for initialization
	void Start () {
        inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();

        currentKey = gameObject.GetComponentInChildren<Text>();

        initialized = false;

    }
	
	// Update is called once per frame
	void Update () {

        if(inputBindings == null)
        {
            inputBindings = inputs.getInputBindings();

        }

        if (inputBindings != null && !initialized)
        {
            if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
            {
                key = inputBindings[command].posAxis;
            }
            else if (inputBindings[command].bidirectional && !positiveDirection)
            {
                key = inputBindings[command].negAxis;
            }

            initialized = true;
        }

        currentKey.text = key.ToUpper();
	}
}
