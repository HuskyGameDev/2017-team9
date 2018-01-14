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
		/*
		InputManager myTar = (InputManager)target;
		
		//The following adds a section to input manager
		//To compactly display what is going on with 
		//The input

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Buttons\t| Current\t| Previous");
		GUILayout.Label("A\t| "			 + myTar.currentButtons[(int)InputManager.ControllerButton.A]				 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.A]);
		GUILayout.Label("B\t| "			 + myTar.currentButtons[(int)InputManager.ControllerButton.B]				 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.B]);
		GUILayout.Label("X\t| "			 + myTar.currentButtons[(int)InputManager.ControllerButton.X]				 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.X]);
		GUILayout.Label("Y\t| "			 + myTar.currentButtons[(int)InputManager.ControllerButton.Y]				 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Y]);
		GUILayout.Label("LeftBump\t| "	 + myTar.currentButtons[(int)InputManager.ControllerButton.LeftBumper]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.LeftBumper]);
		GUILayout.Label("RightBump\t| "	 + myTar.currentButtons[(int)InputManager.ControllerButton.RightBumper]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.RightBumper]);
		GUILayout.Label("LeftStick\t| "	 + myTar.currentButtons[(int)InputManager.ControllerButton.LeftStick]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.LeftStick]);
		GUILayout.Label("RightStick\t| " + myTar.currentButtons[(int)InputManager.ControllerButton.RightStick]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.RightStick]);
		GUILayout.Label("Back\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Back]			 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Back]);
		GUILayout.Label("Start\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Start]			 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Start]);
		GUILayout.Label("Dp_Up\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_Up]			 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_Up]);
		GUILayout.Label("Dp_Right\t| "	 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_Right]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_Right]);
		GUILayout.Label("Dp_Down\t| "	 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_Down]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_Down]);
		GUILayout.Label("Dp_Left\t| "	 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_Left]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_Left]);
		GUILayout.Label("Dp_UR\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_UpRight]	 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_UpRight]);
		GUILayout.Label("Dp_DR\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_DownRight]	 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_DownRight]);
		GUILayout.Label("Dp_DL\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_DownLeft]	 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_DownLeft]);
		GUILayout.Label("Dp_UL\t| "		 + myTar.currentButtons[(int)InputManager.ControllerButton.Dpad_UpLeft]		 + "\t| " + myTar.previousButtons[(int)InputManager.ControllerButton.Dpad_UpLeft]);
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
		*/
	}
}
