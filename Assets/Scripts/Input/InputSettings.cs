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
		public KeyCode Forward;
		[SerializeField]
		public KeyCode Backward;
		[SerializeField]
		public KeyCode LeftStrafe;
		[SerializeField]
		public KeyCode RightStrafe;

		[SerializeField]
		public KeyCode Interact1;
		[SerializeField]
		public KeyCode Interact2;
		[SerializeField]
		public KeyCode Menu;
		[SerializeField]
		public KeyCode Cancel;

	}


	[System.Serializable]
	public class ControllerSettings {
		[SerializeField]
		public InputManager.Axis Forward;
		[SerializeField]
		public InputManager.Axis Strafe;
		[SerializeField]
		public InputManager.Axis Yaw;
		[SerializeField]
		public InputManager.Axis Pitch;

		[SerializeField]
		public InputManager.ControllerButton Interact1;
		[SerializeField]
		public InputManager.ControllerButton Interact2;
		[SerializeField]
		public InputManager.ControllerButton Menu;
		[SerializeField]
		public InputManager.ControllerButton Cancel;
	}
}
