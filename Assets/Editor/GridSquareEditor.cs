using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridSquare))]
public class GridSquareEditor : Editor {

	bool showDefault = false;

	public override void OnInspectorGUI() {
		GridSquare myScript = (GridSquare)target;

		myScript.type = (GridSquare.GridType)EditorGUILayout.EnumPopup("Component:", myScript.type);
		string[] labels = new string[] { "Up      ", "Right  ", "Down ","Left     "};
		for (int i = 0; i < labels.Length; i++) {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(labels[i], new GUILayoutOption[] { GUILayout.Width(40) });

			myScript.socketState[i] = (GridSquare.SocketState)EditorGUILayout.EnumPopup("", myScript.socketState[i], new GUILayoutOption[] { GUILayout.Width(75) });
			myScript.neighbors[i] = EditorGUILayout.ObjectField(myScript.neighbors[i], typeof(GridSquare), true) as GridSquare;
			if (myScript.neighbors[i] == null) {
				if (GUILayout.Button("Spawn")) {
					myScript.AddNeighbor((GridSquare.GridDirection)i);
				}
			}
			//GUILayout.Label("" + ((myScript.lines[i] != null) ? myScript.line[i].GetColor().ToString() : "") ); 
			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Rebuild Square")) {
			myScript.RebuildSquare();
		}

		showDefault = EditorGUILayout.Foldout(showDefault, "Show Unity Default", true);
		if (showDefault)
				base.OnInspectorGUI();



	}
}
