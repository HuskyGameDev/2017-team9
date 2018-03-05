using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCubeGenerator))]
public class GridCubeGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		//Call base so that if InputManager gets changes
		//They will be reflected in the editor properly
		base.OnInspectorGUI();

		GridCubeGenerator myScript = (GridCubeGenerator)target;
		if (GUILayout.Button("Create")) {
			myScript.GenerateCube();
		}
		if (GUILayout.Button("Clear")) {
			myScript.ClearCube();
		}
	}
}
