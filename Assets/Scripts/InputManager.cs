﻿/*  Pipes custom input to Unity's built in Input Manager.
    This will make it possible to all players to customize 
    controls in game
*/


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public int hi = 0;
    public List<InspectorInputPreset> inspectorPresets;
    private List<Dictionary<string, InputBinding>> inputPresets;
    private bool isOnWindows = false;

    private int presetIndex = 0;

	// Use this for initialization
	void Start () {
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
           isOnWindows = true;
        #endif



        inputPresets = new List<Dictionary<string, InputBinding>>();
        inputPresets.Add(new Dictionary<string, InputBinding>());


        inspectorToDicts();

    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        ///delete this after we have made an input options menu
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //Switch to keyboard and mouse
            presetIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //switch to gamepad
            presetIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //switch to joystick
            presetIndex = 2;
        }
    }

    public float getInput(string name)
    {
        /*  Returns float value of input.  Good for analog, mouse, and/or continuous input*/
        
        float posAxis;
        float negAxis = 0;
        float value;

        if (inputPresets == null || inputPresets[presetIndex] == null)
        {
            return 0;
        }

        if (!inputPresets[presetIndex].ContainsKey(name))
        {
            //Debug.Log(name + " not bound.");
            return 0;
        }
        InputBinding input = inputPresets[presetIndex][name];

        posAxis = Input.GetAxis(input.posAxis);
        //Debug.Log(posAxis);

        if (input.bidirectional)
        {
            negAxis = Input.GetAxis(input.negAxis);
        }

        if (input.bidirectional && Math.Abs(negAxis) > posAxis)
        {
            value = -1 * negAxis;
        }
        else
        {
            value = posAxis;
        }


        if (Math.Abs(value) <= input.dead)
        {
            value = 0;
            return value;
        }

        value *= input.sensitivity;
        if (input.invert)
        {
            value *= -1;
        }
        /*if (value != 0)
            Debug.Log(name + value);*/
        return value;
    }

    public bool getInputDown(string name)
    {
        /*  Returns true if input was pressed. Only checks positive input.*/
        if (!inputPresets[presetIndex].ContainsKey(name))
        {
            //Debug.Log(name + " not bound.");
            return false;
        }
        InputBinding input = inputPresets[presetIndex][name];

        if ((presetIndex != 1 && Input.GetButtonDown(input.posAxis)) || (presetIndex == 1 && Input.GetKeyDown(input.posAxis)))
        {
            return true;
        }
        return false;
    }

    public bool getInputUp(string name)
    {
        /*  Returns true if input was released.  Only checks positive input. */
        if (!inputPresets[presetIndex].ContainsKey(name))
        {
            //Debug.Log(name + " not bound.");
            return false;
        }
        InputBinding input = inputPresets[presetIndex][name];

        if (Input.GetButtonUp(input.posAxis))
        {
            return true;
        }
        return false;
    }

    public void inspectorToDicts()
    {
        if (inputPresets == null)
        {
            inputPresets = new List<Dictionary<string, InputBinding>>();
            inputPresets.Add(new Dictionary<string, InputBinding>());
        }

        while (inputPresets.Count < inspectorPresets.Count)
        {
            inputPresets.Add(new Dictionary<string, InputBinding>());
        }
        //for each preset
        for (int i = 0; i < inspectorPresets.Count; ++i)
        {
            //for each input
            foreach (InputBinding input in inspectorPresets[i].bindings)
            {
                if (!inputPresets[i].ContainsKey(input.name))
                {
                    inputPresets[i].Add(input.name, input);
                }
                else
                {
                    inputPresets[i][name] = input;
                }
            }
        }
    }

    public void nextPreset()
    {
        if (presetIndex <= inputPresets.Count)
        {
            presetIndex++;
        }
        else
        {
            presetIndex = 0;
        }
    }

    public void prevPreset()
    {
        if (presetIndex > 0)
        {
            presetIndex--;
        }
        else
        {
            presetIndex = inputPresets.Count - 1;
        }
    }

    public string getPresetName()
    {
        return inspectorPresets[presetIndex].presetName;
    }

}

[Serializable]
public class InputBinding
{
    public string name;                 //Name of input binding
    public bool bidirectional;          //Does the input have different keys for positive and negative directions?
    public string posAxis;              //Unity input for positive direction
    public string negAxis;              //Unity input for negative direction
    public float dead;                  //Dead zone
    public float sensitivity;           //Multipier for sensitivity
    public bool invert;                 //Invert input values.

    public InputBinding()
    {
        name = "";
        bidirectional = false;
        posAxis = "";
        negAxis = "";
        dead = 0.1f;
        sensitivity = 1;
        invert = false;
    }
}

[Serializable]
public class InspectorInputPreset
{
    public string presetName;
    [SerializeField]
    public List<InputBinding> bindings;

    public InspectorInputPreset()
    {
        presetName = "New Preset";
        bindings = new List<InputBinding>();
    }
}
