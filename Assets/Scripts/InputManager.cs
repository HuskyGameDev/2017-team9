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


	//The Movement enum is a helper method to determine what kind of movement on an axis you are trying to read as a button
	//There is effectivly only negative and positive, but I used Left/Right as well as Forward/Backward to make the desired input more readable.
	public enum Movement { Left = 0, Right = 1, Forward = 1, Backward = 0 }

	//This enum maps named Axis to indicies of the stored numbers.
	public enum Axis { LeftHorizontal = 0, LeftVertical = 1, RightHorizontal = 2, RightVertical = 3, LeftTrigger = 4, RightTrigger = 5, DpadHorizontal = 6, DpadVertical = 7 }

	//Stores this frames as well as last frames axis input 
	public float[] previousAxis = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
	public float[] currentAxis = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	//Stores the relation of button names to int (18 buttons!)
	public enum Button {A=0, B=1, X=2, Y=3, LeftBumper=4, RightBumper=5, LeftStick=6, RightStick=7, Back=8, Start=9,
		Dpad_Up =10, Dpad_Right = 11, Dpad_Down = 12, Dpad_Left = 13, Dpad_UpRight = 14, Dpad_DownRight = 15, Dpad_DownLeft = 16, Dpad_UpLeft = 17 }

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

		currentAxis[(int)Axis.LeftHorizontal		] = Input.GetAxis("LeftHorizontal");
		currentAxis[(int)Axis.LeftVertical			] = Input.GetAxis("LeftVertical");

		currentAxis[(int)Axis.RightHorizontal		] = Input.GetAxis("RightHorizontal");
		currentAxis[(int)Axis.RightVertical			] = Input.GetAxis("RightVertical");

		currentAxis[(int)Axis.LeftTrigger			] = Input.GetAxis("LeftTrigger");
		currentAxis[(int)Axis.RightTrigger			] = Input.GetAxis("RightTrigger");

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

	//Returns a button (1-8) depending on the axis input of the Dpad, -1 for no press
	private static int DpadToButton(float DpadH, float DpadV) {
		if (DpadV > .9f) {
			//Up
			return 10;
		}
		else if (DpadH > .9f) {
			//Right
			return 11;
		}
		else if (DpadV < -.9f) {
			//Down
			return 12;
		}
		else if (DpadH < -.9f) {
			//Left
			return 13;
		}
		else if (DpadV > .7f && DpadH > .7f) {
			//UpRight
			return 14;
		}
		else if (DpadV < -.7f && DpadH > .7f) {
			//DownRight
			return 15;
		}
		else if (DpadV < -.7f && DpadH < -.7f) {
			//DownLeft
			return 16;
		}
		else if (DpadV > .7f && DpadH < -.7f) {
			//UpLeft
			return 17;
		}

		//NoButton
		return -1;
	}

	private static bool DpadButton(Button button) {
		return (int)button == DpadToButton(instance.currentAxis[(int)Axis.DpadHorizontal], instance.currentAxis[(int)Axis.DpadVertical]);
	}


}
