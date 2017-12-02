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
	public float moveDead = 0.7f; // CANNOT BE 0
	public float mouseSensitivity = 0.50f;
	public bool invertedMouse = false;
	public InputSettings settings;

	//The Movement enum is a helper method to determine what kind of movement on an axis you are trying to read as a button
	//There is effectivly only negative and positive, but I used Left/Right as well as Forward/Backward to make the desired input more readable.
	public enum Movement { Left = 0, Right = 1, Forward = 1, Backward = 0 }

	//This enum maps named Axis to indicies of the stored numbers.
	public enum Axis { LeftHorizontal = 0, LeftVertical = 1, RightHorizontal = 2, RightVertical = 3, LeftTrigger = 4, RightTrigger = 5, DpadHorizontal = 6, DpadVertical = 7 }
	public static readonly string[] AxisNames = { "LeftHorizontal", "LeftVertical", "RightHorizontal", "RightVertical", "eftTrigger", "RightTrigger", "DpadHorizontal", "DpadVertical" };

	//Stores the relation of button names to int (18 buttons!)
	public enum Button {
		nul = -1, A = 0, B = 1, X = 2, Y = 3, LeftBumper = 4, RightBumper = 5, LeftStick = 6, RightStick = 7, Back = 8, Start = 9,
		Dpad_Up = 10, Dpad_Right = 11, Dpad_Down = 12, Dpad_Left = 13, Dpad_UpRight = 14, Dpad_DownRight = 15, Dpad_DownLeft = 16, Dpad_UpLeft = 17
	}

	//Stores this frames as well as last frames axis input 
	public float[] previousAxis = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
	public float[] currentAxis = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	//Stores this frames as well as last frames button input 
	public bool[] previousButtons = {
		false, false, false, false, false,
		false, false, false, false, false,
		false, false, false, false, false,
		false, false, false};
	public bool[] currentButtons = {
		false, false, false, false, false,
		false, false, false, false, false,
		false, false, false, false, false,
		false, false, false};
	
	// Use this for initialization
	void Awake () {
		if (instance != null) {
			Debug.LogError("You have multiple input managers: \n" + this.transform.name + ", " + instance.transform.name);
			Debug.Break();
		}
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		//Update Axis
		previousAxis = currentAxis;

		currentAxis[(int)Axis.LeftHorizontal		] = Input.GetAxis("LeftHorizontal") + GetAxisFromKeyboard(Axis.LeftHorizontal);
		currentAxis[(int)Axis.LeftVertical			] = Input.GetAxis("LeftVertical") + GetAxisFromKeyboard(Axis.LeftVertical);

		currentAxis[(int)Axis.RightHorizontal		] = Input.GetAxis("RightHorizontal") + GetAxisFromKeyboard(Axis.RightHorizontal);
		currentAxis[(int)Axis.RightVertical			] = Input.GetAxis("RightVertical") + GetAxisFromKeyboard(Axis.RightVertical);

		currentAxis[(int)Axis.LeftTrigger			] = Input.GetAxis("LeftTrigger") + GetAxisFromKeyboard(Axis.LeftTrigger);
		currentAxis[(int)Axis.RightTrigger			] = Input.GetAxis("RightTrigger") + GetAxisFromKeyboard(Axis.RightTrigger);

		currentAxis[(int)Axis.DpadHorizontal		] = Input.GetAxis("DpadHorizontal");
		currentAxis[(int)Axis.DpadVertical			] = Input.GetAxis("DpadVertical");

		//Update Buttons
		previousButtons = currentButtons;

		currentButtons[(int)Button.A				] = Input.GetButton("A");
		currentButtons[(int)Button.B				] = Input.GetButton("B");
		currentButtons[(int)Button.X				] = Input.GetButton("X");
		currentButtons[(int)Button.Y				] = Input.GetButton("Y");

		currentButtons[(int)Button.LeftBumper		] = Input.GetButton("LeftBumper");
		currentButtons[(int)Button.RightBumper		] = Input.GetButton("RightBumper");

		currentButtons[(int)Button.LeftStick		] = Input.GetButton("LeftStick");
		currentButtons[(int)Button.RightStick		] = Input.GetButton("RightStick");

		currentButtons[(int)Button.Back				] = Input.GetButton("Back");
		currentButtons[(int)Button.Start			] = Input.GetButton("Start");

		currentButtons[(int)Button.Dpad_Up			] = DpadButton(Button.Dpad_Up);
		currentButtons[(int)Button.Dpad_Right		] = DpadButton(Button.Dpad_Right);
		currentButtons[(int)Button.Dpad_Down		] = DpadButton(Button.Dpad_Down);
		currentButtons[(int)Button.Dpad_Left		] = DpadButton(Button.Dpad_Left);
		currentButtons[(int)Button.Dpad_UpRight		] = DpadButton(Button.Dpad_UpRight);
		currentButtons[(int)Button.Dpad_DownRight	] = DpadButton(Button.Dpad_DownRight);
		currentButtons[(int)Button.Dpad_DownLeft	] = DpadButton(Button.Dpad_DownLeft);
		currentButtons[(int)Button.Dpad_UpLeft		] = DpadButton(Button.Dpad_UpLeft);
	}

	public static bool GetButton(Button button) {
		return instance.currentButtons[(int)button];
	}

	public static bool GetButtonDown(Button button) {
		return instance.currentButtons[(int)button] && !instance.previousButtons[(int)button];
	}

	public static bool GetButtonUp(Button button) {
		return !instance.currentButtons[(int)button] && instance.previousButtons[(int)button];
	}

	public static float GetAxis(Axis input) {
		return instance.currentAxis[(int)input];
	}


	public static bool GetAxisAsButton(Axis input, Movement direction) {
		return ((int)direction == 1) ? instance.currentAxis[(int)input] >= instance.moveDead : instance.currentAxis[(int)input] <= -instance.moveDead;
	}

	public static bool GetAxisAsButtonDown(Axis input, Movement direction) {
		if ((int)direction == 1) {
			return (instance.currentAxis[(int)input] > instance.moveDead && instance.previousAxis[(int)input] < instance.moveDead);
		}
		else {
			return (instance.currentAxis[(int)input] < -instance.moveDead && instance.previousAxis[(int)input] > -instance.moveDead);
		}
	}

	public static bool GetAxisAsButtonUp(Axis input, Movement direction) {
		if ((int)direction == 1) {
			return (instance.previousAxis[(int)input] >= instance.moveDead && instance.currentAxis[(int)input] < instance.moveDead);
		}
		else {
			return (instance.previousAxis[(int)input] <= -instance.moveDead && instance.currentAxis[(int)input] > -instance.moveDead);
		}
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
	private static Button DpadToButton(float DpadH, float DpadV) {
		if (DpadV > .9f) {
			//Up
			return Button.Dpad_Up;
		}
		else if (DpadH > .9f) {
			//Right
			return Button.Dpad_Right;
		}
		else if (DpadV < -.9f) {
			//Down
			return Button.Dpad_Down;
		}
		else if (DpadH < -.9f) {
			//Left
			return Button.Dpad_Left;
		}
		else if (DpadV > .7f && DpadH > .7f) {
			//UpRight
			return Button.Dpad_UpRight;
		}
		else if (DpadV < -.7f && DpadH > .7f) {
			//DownRight
			return Button.Dpad_DownRight;
		}
		else if (DpadV < -.7f && DpadH < -.7f) {
			//DownLeft
			return Button.Dpad_DownLeft;
		}
		else if (DpadV > .7f && DpadH < -.7f) {
			//UpLeft
			return Button.Dpad_UpLeft;
		}
		return Button.nul;
	}

	private static bool DpadButton(Button button) {
		return button == DpadToButton(instance.currentAxis[(int)Axis.DpadHorizontal], instance.currentAxis[(int)Axis.DpadVertical]);
	}


}
