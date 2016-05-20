using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(wsPlatformCharacter))]
public class wsPlatformCharacter_Editor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
	}//	End method OnInspectorGUI

}//	End class wsPlatformCharacter_Editor
