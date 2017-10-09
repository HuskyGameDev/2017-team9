using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor {
	public override void OnInspectorGUI() {
		//Call base so that if InputManager gets changes
		//They will be reflected in the editor properly
		base.OnInspectorGUI();

		InputManager myTar = (InputManager)target;

		//The following adds a section to input manager
		//To compactly display what is going on with 
		//The input

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Buttons\t| Current\t| Previous");
		GUILayout.Label("A\t| "			 + myTar.currentButtons[(int)InputManager.Button.A]				 + "\t| " + myTar.previousButtons[(int)InputManager.Button.A]);
		GUILayout.Label("B\t| "			 + myTar.currentButtons[(int)InputManager.Button.B]				 + "\t| " + myTar.previousButtons[(int)InputManager.Button.B]);
		GUILayout.Label("X\t| "			 + myTar.currentButtons[(int)InputManager.Button.X]				 + "\t| " + myTar.previousButtons[(int)InputManager.Button.X]);
		GUILayout.Label("Y\t| "			 + myTar.currentButtons[(int)InputManager.Button.Y]				 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Y]);
		GUILayout.Label("LeftBump\t| "	 + myTar.currentButtons[(int)InputManager.Button.LeftBumper]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.LeftBumper]);
		GUILayout.Label("RightBump\t| "	 + myTar.currentButtons[(int)InputManager.Button.RightBumper]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.RightBumper]);
		GUILayout.Label("LeftStick\t| "	 + myTar.currentButtons[(int)InputManager.Button.LeftStick]		 + "\t| " + myTar.previousButtons[(int)InputManager.Button.LeftStick]);
		GUILayout.Label("RightStick\t| " + myTar.currentButtons[(int)InputManager.Button.RightStick]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.RightStick]);
		GUILayout.Label("Back\t| "		 + myTar.currentButtons[(int)InputManager.Button.Back]			 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Back]);
		GUILayout.Label("Start\t| "		 + myTar.currentButtons[(int)InputManager.Button.Start]			 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Start]);
		GUILayout.Label("Dp_Up\t| "		 + myTar.currentButtons[(int)InputManager.Button.Dpad_Up]		 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_Up]);
		GUILayout.Label("Dp_Right\t| "	 + myTar.currentButtons[(int)InputManager.Button.Dpad_Right]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_Right]);
		GUILayout.Label("Dp_Down\t| "	 + myTar.currentButtons[(int)InputManager.Button.Dpad_Down]		 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_Down]);
		GUILayout.Label("Dp_Left\t| "	 + myTar.currentButtons[(int)InputManager.Button.Dpad_Left]		 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_Left]);
		GUILayout.Label("Dp_UR\t| "		 + myTar.currentButtons[(int)InputManager.Button.Dpad_UpRight]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_UpRight]);
		GUILayout.Label("Dp_DR\t| "		 + myTar.currentButtons[(int)InputManager.Button.Dpad_DownRight] + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_DownRight]);
		GUILayout.Label("Dp_DL\t| "		 + myTar.currentButtons[(int)InputManager.Button.Dpad_DownLeft]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_DownLeft]);
		GUILayout.Label("Dp_UL\t| "		 + myTar.currentButtons[(int)InputManager.Button.Dpad_UpLeft]	 + "\t| " + myTar.previousButtons[(int)InputManager.Button.Dpad_UpLeft]);
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		GUILayout.Label("Axis\t| Current\t| Previous");
		GUILayout.Label("LeftHoriz\t| "		+ myTar.currentAxis[(int)InputManager.Axis.LeftHorizontal].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.LeftHorizontal].ToString("0.00"));
		GUILayout.Label("LeftVert\t| "		+ myTar.currentAxis[(int)InputManager.Axis.LeftVertical].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.LeftVertical].ToString("0.00"));
		GUILayout.Label("RightHoriz\t| "	+ myTar.currentAxis[(int)InputManager.Axis.RightHorizontal].ToString("0.00") + "\t| " + myTar.previousAxis[(int)InputManager.Axis.RightHorizontal].ToString("0.00"));
		GUILayout.Label("RightVert\t| "		+ myTar.currentAxis[(int)InputManager.Axis.RightVertical].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.RightVertical].ToString("0.00"));
		GUILayout.Label("LeftTrig\t| "		+ myTar.currentAxis[(int)InputManager.Axis.LeftTrigger].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.LeftTrigger].ToString("0.00"));
		GUILayout.Label("RightTrig\t| "		+ myTar.currentAxis[(int)InputManager.Axis.RightTrigger].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.RightTrigger].ToString("0.00"));
		GUILayout.Label("DpHoriz\t| "		+ myTar.currentAxis[(int)InputManager.Axis.DpadHorizontal].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.DpadHorizontal].ToString("0.00"));
		GUILayout.Label("DpVert\t| "		+ myTar.currentAxis[(int)InputManager.Axis.DpadVertical].ToString("0.00")	 + "\t| " + myTar.previousAxis[(int)InputManager.Axis.DpadVertical].ToString("0.00"));
		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal();

	}
}
