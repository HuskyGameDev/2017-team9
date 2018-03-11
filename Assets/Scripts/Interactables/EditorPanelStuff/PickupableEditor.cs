using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pickupable))]
public class PickupableEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		Pickupable myScript = (Pickupable)target;

		if (GUILayout.Button("Update Visuals")) {
			myScript.GenerateVisuals();
		}

		//myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		//EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
	}
}
