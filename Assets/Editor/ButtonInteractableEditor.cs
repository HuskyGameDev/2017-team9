using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonInteractable))]
public class ButtonInteractableEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		ButtonInteractable myScript = (ButtonInteractable)target;

		if (GUILayout.Button("Update Visuals")) {
			myScript.GenerateVisual();
		}
	}
}
