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
        /*  Pipes custom inputs through Unity's input manager.*/
        
        float posAxis;
        float negAxis = 0;
        float value;

        if (!inputs.ContainsKey(name))
        {
            Debug.Log(name + " not bound.");
            return 0;
        }
        InputBinding input = inputs[name];

        posAxis = Input.GetAxis(input.positiveAxis);
        Debug.Log(posAxis);

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

        return value;
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

