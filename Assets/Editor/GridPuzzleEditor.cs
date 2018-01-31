using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridPuzzle))]
public class GridPuzzleEditor : Editor {

	public override void OnInspectorGUI() {
		//Call base so that if InputManager gets changes
		//They will be reflected in the editor properly
		base.OnInspectorGUI();

		GridPuzzle myScript = (GridPuzzle)target;
		if (GUILayout.Button("Build Grid")) {
			myScript.GenerateGrid();
		}
		if (GUILayout.Button("Destroy Grid")) {
			myScript.DestroyGrid();
		}
	}
}