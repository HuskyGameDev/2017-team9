using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputSettings {

	[SerializeField]
	public ControllerSettings controller = new ControllerSettings();

	[SerializeField]
	public KeyboardSettings keyboard = new KeyboardSettings();



	[System.Serializable]
	public class KeyboardSettings {
		[SerializeField]
		KeyCode Forward;
		[SerializeField]
		KeyCode Backward;
		[SerializeField]
		KeyCode LeftStrafe;
		[SerializeField]
		KeyCode RightStrafe;

		[SerializeField]
		KeyCode Interact1;
		[SerializeField]
		KeyCode Interact2;
		[SerializeField]
		KeyCode Menu;
		[SerializeField]
		KeyCode Cancel;

	}


	[System.Serializable]
	public class ControllerSettings {
		[SerializeField]
		InputManager.Axis Forward;
		[SerializeField]
		InputManager.Axis Strafe;
		[SerializeField]
		InputManager.Axis Yaw;
		[SerializeField]
		InputManager.Axis Pitch;

		[SerializeField]
		InputManager.Button Interact1;
		[SerializeField]
		InputManager.Button Interact2;
		[SerializeField]
		InputManager.Button Menu;
		[SerializeField]
		InputManager.Button Cancel;
	}
}
