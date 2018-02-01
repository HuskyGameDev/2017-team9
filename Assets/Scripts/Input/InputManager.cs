using UnityEngine;


//This class provides axis input updated every frame. 
//It also has methods to caluclate axis movements as buttons.
//
//This class is currently dependent on how Windows interprets Dpad.
//On windows Dpad is an axis, other operating systems abstract this as a button (which it should be)
//This has NOT been implemented
public class InputManager : MonoBehaviour {

	public static InputManager instance;

	//This defines the point at which we consider an axis has been used as a button
	[Range(0.19f,1.0f)]
	public float moveDead = 0.7f;
	public float mouseSensitivity = 0.50f;
	public bool invertedMouse = false;
	public InputSettings settings;

	//The Movement enum is a helper method to determine what kind of movement on an axis you are trying to read as a button
	//There is effectivly only negative and positive, but I used Left/Right as well as Forward/Backward to make the desired input more readable.
	public enum Movement { Left = 0, Right = 1, Forward = 1, Backward = 0 }

	//This enum maps named Axis to indicies of the stored numbers.
	public enum Axis { LeftHorizontal = 0, LeftVertical = 1, RightHorizontal = 2, RightVertical = 3, LeftTrigger = 4, RightTrigger = 5, DpadHorizontal = 6, DpadVertical = 7 }
	public static readonly string[] AxisNames = { "LeftHorizontal", "LeftVertical", "RightHorizontal", "RightVertical", "LeftTrigger", "RightTrigger", "DpadHorizontal", "DpadVertical" };

	//Stores the relation of button names to int (18 buttons!)
	public enum ControllerButton {
		nul = -1, A = 0, B = 1, X = 2, Y = 3, LeftBumper = 4, RightBumper = 5, LeftStick = 6, RightStick = 7, Back = 8, Start = 9,
		Dpad_Up = 10, Dpad_Right = 11, Dpad_Down = 12, Dpad_Left = 13, Dpad_UpRight = 14, Dpad_DownRight = 15, Dpad_DownLeft = 16, Dpad_UpLeft = 17
	}
	public static readonly string[] ControllerButtonToString = { "A", "B", "X","Y", "LeftBumper", "RightBumper", "LeftStick", "RightStick", "Back", "Start"};


	//The overall list of possible game interactions. This excludes things for character and mouse movement. Below it are two arrays that map the proper settings.
	//DO NOT FORGET: you have to update the two arrays in the awake method below.
	public enum GameButton {Interact1, Interact2, Menu, Cancel, CameraLock}
	private ControllerButton[] GameButtonToControllerButton;
	private KeyCode[] GameButtonToKeycode;

	// Use this for initialization
	void Awake () {
		if (instance != null) {
			Debug.LogError("You have multiple input managers: \n" + this.transform.name + ", " + instance.transform.name);
			Debug.Break();
		}
		instance = this;
		//Link the keyboard settings
		GameButtonToControllerButton = new ControllerButton[]	{ settings.controller.Interact1	, settings.controller.Interact2	, settings.controller.Menu	, settings.controller.Cancel, settings.controller.cameraLock };
		GameButtonToKeycode = new KeyCode[]						{ settings.keyboard.Interact1	, settings.keyboard.Interact2	, settings.keyboard.Menu	, settings.keyboard.Cancel, settings.keyboard.cameraLock };
	}

	public static float GetAxis(Axis input) {
		if (input == Axis.LeftHorizontal)
			return Input.GetAxis(AxisNames[(int)Axis.LeftHorizontal]) + GetAxisFromKeyboard(Axis.LeftHorizontal);
		else if(input == Axis.LeftVertical)
			return Input.GetAxis(AxisNames[(int)Axis.LeftVertical]) + GetAxisFromKeyboard(Axis.LeftVertical);
		else if(input == Axis.RightHorizontal)
			return Input.GetAxis(AxisNames[(int)Axis.RightHorizontal]) + GetAxisFromKeyboard(Axis.RightHorizontal);
		else if(input == Axis.RightVertical)
			return Input.GetAxis(AxisNames[(int)Axis.RightVertical]) + GetAxisFromKeyboard(Axis.RightVertical);
		else if(input == Axis.LeftTrigger)
			return Input.GetAxis(AxisNames[(int)Axis.LeftTrigger]) + GetAxisFromKeyboard(Axis.LeftTrigger);
		else if(input == Axis.RightTrigger)
			return Input.GetAxis(AxisNames[(int)Axis.RightTrigger]) + GetAxisFromKeyboard(Axis.RightTrigger);
		else if(input == Axis.DpadHorizontal)
			return Input.GetAxis(AxisNames[(int)Axis.DpadHorizontal]);
		else if(input == Axis.DpadVertical)
			return Input.GetAxis(AxisNames[(int)Axis.DpadVertical]);
		else
			return 0;
	}



	private static float GetAxisFromKeyboard(Axis axis) {
		float ret = 0.0f;
		//Check all Axis using else if so that we only return the information for one
		if (axis == Axis.LeftHorizontal) {
			//So we check if these two keys are pressed and move in that direction
			//if both are pressed it defaults back to 0 and no movement happens
			if (Input.GetKey(KeyCode.A) ) {
				ret -= 1;
			}
			if (Input.GetKey(KeyCode.D)) {
				ret += 1;
			}
		}
		else if (axis == Axis.LeftVertical) {
			if (Input.GetKey(KeyCode.S)) {
				ret -= 1;
			}
			if (Input.GetKey(KeyCode.W)) {
				ret += 1;
			}
		}
		else if (axis == Axis.RightHorizontal) {
			ret = Input.GetAxis("Mouse X") * instance.mouseSensitivity;
		}
		else if (axis == Axis.RightVertical) {
			ret = Input.GetAxis("Mouse Y") * instance.mouseSensitivity;
			if (instance.invertedMouse == false) ret *= -1; 
		}
		else if (axis == Axis.RightTrigger) {
			ret = (Input.GetMouseButton(1)) ? 1.0f : 0.0f;
		}
		else if (axis == Axis.LeftTrigger) {
			ret = (Input.GetMouseButton(0)) ? 1.0f : 0.0f;
		}

		return ret;
	}

	//Returns a button (1-8) depending on the axis input of the Dpad, -1 for no press
	private static ControllerButton DpadToButton(float DpadH, float DpadV) {
		if (DpadV > .9f) {
			//Up
			return ControllerButton.Dpad_Up;
		}
		else if (DpadH > .9f) {
			//Right
			return ControllerButton.Dpad_Right;
		}
		else if (DpadV < -.9f) {
			//Down
			return ControllerButton.Dpad_Down;
		}
		else if (DpadH < -.9f) {
			//Left
			return ControllerButton.Dpad_Left;
		}
		else if (DpadV > .7f && DpadH > .7f) {
			//UpRight
			return ControllerButton.Dpad_UpRight;
		}
		else if (DpadV < -.7f && DpadH > .7f) {
			//DownRight
			return ControllerButton.Dpad_DownRight;
		}
		else if (DpadV < -.7f && DpadH < -.7f) {
			//DownLeft
			return ControllerButton.Dpad_DownLeft;
		}
		else if (DpadV > .7f && DpadH < -.7f) {
			//UpLeft
			return ControllerButton.Dpad_UpLeft;
		}
		return ControllerButton.nul;
	}

	private static bool DpadButton(ControllerButton button) {
		return button == DpadToButton(Input.GetAxis(AxisNames[(int)Axis.DpadHorizontal]), Input.GetAxis(AxisNames[(int)Axis.DpadVertical]));
	}



	/// <summary>
	/// The following methods encapsulate the comination of the two input methods.
	/// </summary>
	/// <param name="button"></param>
	/// <returns></returns>
	public static bool GetGameButton(GameButton button) {
		if (Input.GetButton(ControllerButtonToString[(int)instance.GameButtonToControllerButton[(int)button]]) || Input.GetKey(instance.GameButtonToKeycode[(int)button])) {
			return true;
		}
		return false;
	}
	public static bool GetGameButtonDown(GameButton button) {
		if (Input.GetButtonDown(ControllerButtonToString[(int)instance.GameButtonToControllerButton[(int)button]]) || Input.GetKeyDown(instance.GameButtonToKeycode[(int)button])) {
			return true;
		}
		return false;

	}
	public static bool GetGameButtonUp(GameButton button) {
		if (Input.GetButtonUp(ControllerButtonToString[(int)instance.GameButtonToControllerButton[(int)button]]) || Input.GetKeyUp(instance.GameButtonToKeycode[(int)button])) {
			return true;
		}
		return false;

	}

}
