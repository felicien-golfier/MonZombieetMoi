using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor {

	public override void OnInspectorGUI() {
		InputManager inman = (InputManager)target;

		inman.control_type = (InputManager.ControlType)EditorGUILayout.EnumPopup ("Control Type : ", (System.Enum)inman.control_type);
		inman.body_controls = (InputManager.BodyControl)EditorGUILayout.EnumPopup ("Body Controls : ", (System.Enum)inman.body_controls);

		if (inman.control_type == InputManager.ControlType.FIRST_PERSON) 
			inman.head_controls = (InputManager.HeadControl)EditorGUILayout.EnumPopup ("Head Controls : ", (System.Enum)inman.head_controls);

	}
	
}
