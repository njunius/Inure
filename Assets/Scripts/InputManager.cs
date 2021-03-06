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
            Debug.Log("Keyboard and Mouse");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //switch to gamepad
            presetIndex = 1;
            Debug.Log("Xbox 360 Controller");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //switch to joystick
            presetIndex = 2;
            Debug.Log("Keyboard and Joystick");
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

        input.PreviousValue = input.CurrentValue;

        posAxis = Input.GetAxis(input.posAxis);
        //Debug.Log(posAxis);

        if (input.bidirectional)
        {
            negAxis = -1 * Input.GetAxis(input.negAxis);
            value = posAxis + negAxis;
        }
        else
        {
            value = posAxis;
        }

        /*if (input.bidirectional && Math.Abs(negAxis) > posAxis)
        {
            value = -1 * negAxis;
        }
        else
        {
            value = posAxis;
        }*/


        if (Math.Abs(value) <= input.dead)
        {
            value = 0;
            input.CurrentValue = value;
            return value;
        }

        value *= input.sensitivity;
        if (input.invert)
        {
            value *= -1;
        }
        /*if (value != 0)
            Debug.Log(name + value);*/
        input.CurrentValue = value;
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

        input.PreviousValue = input.CurrentValue;

        if ((presetIndex != 1 && Input.GetButtonDown(input.posAxis)) || (presetIndex == 1 && Input.GetKeyDown(input.posAxis)))
        {
            input.CurrentValue = 1;
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
        input.PreviousValue = input.CurrentValue;

        if (Input.GetButtonUp(input.posAxis)) //  || Input.GetButtonUp(input.posAxisAlt1) || Input.GetButtonUp(input.posAxisAlt2
        {
            input.CurrentValue = 0;
            return true;
        }
        return false;
    }

    public bool getInputUpEnhanced(string name)
    {
        if (!inputPresets[presetIndex].ContainsKey(name))
        {
            //Debug.Log(name + " not bound.");
            return false;
        }
        InputBinding input = inputPresets[presetIndex][name];
        //Debug.Log(name + " cur: " + input.CurrentValue);
        //Debug.Log(name + " prev: " + input.PreviousValue);
        if ((input.PreviousValue == 0 && input.CurrentValue != 0) || (input.CurrentValue == 0 && input.PreviousValue == 0) || 
            (input.CurrentValue != 0 && input.PreviousValue != 0))
        {
            return false;
        }

        return true;
    }

    public string getPosInputName(string name)
    {
        if (!inputPresets[presetIndex].ContainsKey(name))
        {
            Debug.Log(name + " not bound.");
            return "";
        }
        InputBinding input = inputPresets[presetIndex][name];
        return input.posAxis;
    }

    public string getNegInputName(string name)
    {
        if (!inputPresets[presetIndex].ContainsKey(name))
        {
            Debug.Log(name + " not bound.");
            return "";
        }
        InputBinding input = inputPresets[presetIndex][name];
        return input.negAxis;
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

    public void editInputBinding(string inputBinding, string newName, bool isNegative)
    {
        if (!isNegative)
        {
            inputPresets[presetIndex][inputBinding].posAxis = newName;
        }
        else
        {
            inputPresets[presetIndex][inputBinding].negAxis = newName;
        }
    }

    public Dictionary<string, InputBinding> getInputBindings()
    {
        return inputPresets[presetIndex];

        //inputPresets[0]["Vertical"].posAxis;
        //inputPresets[0]["Vertical"].bidirectional;

        /*foreach (InputBinding binding in inputPresets[0].Values)
        {
            binding.name;
            binding.posAxis;
            if (binding.bidirectional)
            {
                bidning.negAxis;
            }
            
        }*/
    }

    public void setInputSensitivity(string inputBinding, float sensitivity)
    {
        float range = inputPresets[presetIndex][inputBinding].maxSensitivity - inputPresets[presetIndex][inputBinding].minSensitivity;
        float amount = range * sensitivity;
        inputPresets[presetIndex][inputBinding].sensitivity = inputPresets[presetIndex][inputBinding].minSensitivity + amount;
    }

    public float getInputSensitivity(string inputBinding)
    {
        float range = inputPresets[presetIndex][inputBinding].maxSensitivity - inputPresets[presetIndex][inputBinding].minSensitivity;
        float amount = inputPresets[presetIndex][inputBinding].sensitivity - inputPresets[presetIndex][inputBinding].minSensitivity;

        return amount / range;
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
    public float minSensitivity;
    public float maxSensitivity;
    public bool invert;                 //Invert input values.
    public string posAxisAlt1;              //Unity input for positive direction
    public string negAxisAlt1;
    public string posAxisAlt2;              //Unity input for positive direction
    public string negAxisAlt2;

    private float previousValue;
    private float currentValue;

    public InputBinding()
    {
        name = "";
        bidirectional = false;
        posAxis = "";
        negAxis = "";
        dead = 0.1f;
        sensitivity = 1;
        invert = false;
        posAxisAlt1 = "";
        negAxisAlt1 = "";
        posAxisAlt2 = "";
        negAxisAlt2 = "";

        previousValue = 0;

    }

    public float PreviousValue
    {
        set { this.previousValue = value; }
        get { return this.previousValue; }
    }

    public float CurrentValue
    {
        set { this.currentValue = value; }
        get { return this.currentValue; }
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

