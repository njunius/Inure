using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerInspector : Editor {

    InputManager im;

    void OnEnabled()
    {
        im = (InputManager)target;

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        im = (InputManager)target;


        /*if (inputs == null)
        {
            inputs = new List<InputBinding>();
            inputs.Add(new InputBinding());
        }
        else
        {
            inputs.Clear();
        }
        if (im.inputs == null)
        {
            im.inputs = new Dictionary<string, InputBinding>();
        }
        foreach (KeyValuePair<string, InputBinding> entry in im.inputs)
        {
            inputs.Add(entry.Value);
        }
        List<InputBinding> editedInputs = new List<InputBinding>();
        InputBinding newInput;

        GUILayout.Label("Inputs");
        foreach (InputBinding input in inputs)
        {
            EditorGUILayout.PropertyField()
            /*newInput = input;
            //EditorGUILayout.Foldout(false,)
            newInput.name = EditorGUILayout.TextField("Name", newInput.name);
            newInput.bidirectional = EditorGUILayout.Toggle("Bidirectional", newInput.bidirectional);
            newInput.positiveAxis = EditorGUILayout.TextField("Positive Axis", newInput.positiveAxis);
            if (newInput.bidirectional)
            {
                newInput.negativeAxis = EditorGUILayout.TextField("Negative Axis", newInput.negativeAxis);
            }
            newInput.dead = EditorGUILayout.FloatField("Dead", newInput.dead);
            newInput.sensitivity = EditorGUILayout.FloatField("Sensitivity", newInput.sensitivity);



            GUILayout.Space(10);
            editedInputs.Add(newInput);*/
        /*}

        inputs = editedInputs;

        if (GUILayout.Button("Add"))
        {
            inputs.Add(new InputBinding());
        }
        if (GUILayout.Button("Clear"))
        {
            inputs.Clear();
            inputs.Add(new InputBinding());
        }


        */
        if (GUI.changed)
        {
            im.inspectorListToDict();
            /*Debug.Log("change");
            im.inputs.Clear();
            foreach (InputBinding input in inputs)
            {
                im.inputs.Add(input.name, input);
            }
            EditorUtility.SetDirty(im);*/
        }
    }


    
}
