using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Receptacle))]
public class ReceptacleEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		Receptacle myScript = (Receptacle)target;

		if (GUILayout.Button("Update Visuals")) {
			myScript.GenerateVisuals();
		}

		//myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		//EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
	}
}
