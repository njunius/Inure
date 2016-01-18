/*  Pipes custom input to Unity's built in Input Manager.
    This will make it possible to all players to customize 
    controls in game
*/


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public List<InputBinding> inspectorInputs;
    Dictionary<string, InputBinding> inputs;

	// Use this for initialization
	void Start () {
        inputs = new Dictionary<string, InputBinding>();

        inspectorListToDict();


    }
	

    public float getInput(string name)
    {
        /*  Returns float value of input.  Good for analog, mouse, and/or continuous input*/
        
        float posAxis;
        float negAxis = 0;
        float value;

        if (inputs == null)
        {
            return 0;
        }

        if (!inputs.ContainsKey(name))
        {
            Debug.Log(name + " not bound.");
            return 0;
        }
        InputBinding input = inputs[name];

        posAxis = Input.GetAxis(input.positiveAxis);
        //Debug.Log(posAxis);

        if (input.bidirectional)
        {
            negAxis = Input.GetAxis(input.negativeAxis);
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
        //Debug.Log("value: " + value);
        return value;
    }

    public bool getInputDown(string name)
    {
        /*  Returns true if input was pressed. Only checks positive input.*/
        if (!inputs.ContainsKey(name))
        {
            Debug.Log(name + " not bound.");
            return false;
        }
        InputBinding input = inputs[name];

        if (Input.GetButtonDown(input.positiveAxis))
        {
            return true;
        }
        return false;
    }

    public bool getInputUp(string name)
    {
        /*  Returns true if input was released.  Only checks positive input. */
        if (!inputs.ContainsKey(name))
        {
            Debug.Log(name + " not bound.");
            return false;
        }
        InputBinding input = inputs[name];

        if (Input.GetButtonUp(input.positiveAxis))
        {
            return true;
        }
        return false;
    }

    public void inspectorListToDict()
    {
        foreach (InputBinding input in inspectorInputs)
        {
            if (!inputs.ContainsKey(input.name))
            {
                inputs.Add(input.name, input);
            }
            else
            {
                inputs[name] = input;
            }
            
        }
    }

}

[Serializable]
public class InputBinding
{
    public string name;                 //Name of input binding
    public bool bidirectional;          //Does the input have different keys for positive and negative directions?
    public string positiveAxis;         //Unity input for positive direction
    public string negativeAxis;         //Unity input for negative direction
    public float dead;                  //Dead zone
    public float sensitivity;           //Multipier for sensitivity

    public InputBinding()
    {
        name = "";
        bidirectional = false;
        positiveAxis = "";
        negativeAxis = "";
        dead = 0.1f;
        sensitivity = 1;
    }
}

