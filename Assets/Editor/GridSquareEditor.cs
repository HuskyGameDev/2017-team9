using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridSquare))]
public class GridSquareEditor : Editor {

	public override void OnInspectorGUI() {
		GridSquare myScript = (GridSquare)target;
		if (GUILayout.Button("Rebuild Square")) {
			myScript.RebuildSquare();
		}

		base.OnInspectorGUI();
	}
}
