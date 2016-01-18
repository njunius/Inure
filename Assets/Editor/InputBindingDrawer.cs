using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomPropertyDrawer(typeof(InputBinding))]
public class InputBindingDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect nameRect = new Rect(position.x, position.y, 30, position.height);
        Rect biRect = new Rect(position.x, position.y + 35, 50, position.height);
        Rect deadRect = new Rect(position.x, position.y + 90, position.width - 90, position.height);

        string name = property.FindPropertyRelative("name").stringValue;
        bool bidirectional = property.FindPropertyRelative("bidirectional").boolValue;
        string positiveAxis = property.FindPropertyRelative("positiveAxis").stringValue;
        string negativeAxis = property.FindPropertyRelative("negativeAxis").stringValue;
        float dead = property.FindPropertyRelative("dead").floatValue;
        float sensitivity = property.FindPropertyRelative("sensitivity").floatValue;


        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        

        name = EditorGUI.TextField(nameRect, "Name", name);
        bidirectional = EditorGUI.Toggle(biRect, "Bidirectional", bidirectional);
        /*if (bidirectional)
        {
            positiveAxis = EditorGUILayout.TextField("Positive Axis", positiveAxis);
            negativeAxis = EditorGUILayout.TextField("Negative Axis", negativeAxis);
        }
        else
        {
            positiveAxis = EditorGUILayout.TextField("Axis", positiveAxis);
        }*/
        dead = EditorGUI.FloatField(deadRect, "Dead", dead);
        //sensitivity = EditorGUILayout.FloatField("Sensitivity", sensitivity);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
