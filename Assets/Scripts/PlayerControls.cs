using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

	public GameObject cam;
	public float moveSpeed = 10.0f;
	public float rotationSpeed = 1.0f;
	public float pitchSpeed = 1.0f;
	public Vector2 pitchBounds = new Vector2(-45,45);
	private float internalRotation = 0.0f;
	private float cameraPitch = 0.0f;

	void Start () {
		//Initialize input
	}

	private void FixedUpdate() {
		handleCamera();
		handleMovement();
	}
	void Update () {
		//DEBUGTEST_INPUT();

		if (Input.GetButtonDown("Y")) {
			cam.GetComponent<CameraManager>().NextRender();
		}
	}

	private void handleMovement() {
		//[TODO] This movement is BAD it force moves the model with no care for anything. this was made just to test the input and should be reworked

		//So we have two inputs that are basically an X and a Y component of a vector.
		//We cannot use these directly because our character model rotates and we need to use these in reference of our character.
		//We can use X (Up and down) as a measure of how much forward we want to move.
		//We can use Y (Left and down) as a measure of how much we want to strafe.
		Vector3 moveDir = (this.transform.forward * Input.GetAxis("LeftVertical")) + (this.transform.right * Input.GetAxis("LeftHorizontal"));
		//Now that we have our desired move direction, we need to normalize it so that our vector length is 1. This avoids diagnol movement being faster.
		moveDir.Normalize();

		this.transform.position = this.transform.position + (moveDir * moveSpeed * Time.deltaTime);
	}

	private void handleCamera() {
		//Camera is handled by rotating our player model left and right
		internalRotation += Input.GetAxis("RightHorizontal") * rotationSpeed * Time.deltaTime;
		this.transform.localRotation = Quaternion.Euler(0.0f, internalRotation, 0.0f);


		//Camera is Handled by rotation our camera up and down.
		cameraPitch += Input.GetAxis("RightVertical") * pitchSpeed * Time.deltaTime;
		if (cameraPitch < pitchBounds.x) cameraPitch = pitchBounds.x;
		if (cameraPitch > pitchBounds.y) cameraPitch = pitchBounds.y;

		cam.transform.localRotation = Quaternion.Euler(cameraPitch, 0.0f, 0.0f);
	}

	private void DEBUGTEST_INPUT() {
		//Stick Movement
		if (Input.GetAxis("LeftHorizontal") != 0.0f) Debug.Log("LeftHorizontal");
		if (Input.GetAxis("RightHorizontal") != 0.0f) Debug.Log("RightHorizontal");
		if (Input.GetAxis("LeftVertical") != 0.0f) Debug.Log("LeftVetical");
		if (Input.GetAxis("RightVertical") != 0.0f) Debug.Log("RightVetical");

		//Button Pad
		if (Input.GetButtonDown("A")) Debug.Log("Click A");
		if (Input.GetButtonDown("B")) Debug.Log("Click B");
		if (Input.GetButtonDown("X")) Debug.Log("Click X");
		if (Input.GetButtonDown("Y")) Debug.Log("Click Y");

		//Bumper
		if (Input.GetButtonDown("LeftBumper")) Debug.Log("LeftBumper");
		if (Input.GetButtonDown("RightBumper")) Debug.Log("RightBumper");

		//Stick Clicks
		if (Input.GetButtonDown("LeftStick")) Debug.Log("LeftStick");
		if (Input.GetButtonDown("RightStick")) Debug.Log("RightStick");

		//Trigger's
		if (Input.GetAxis("LeftTrigger") != 0.0f) Debug.Log(Input.GetAxis("LeftTrigger"));
		if (Input.GetAxis("RightTrigger") != 0.0f) Debug.Log(Input.GetAxis("RightTrigger"));

		//Dpad // UP/DOWN/LEFT/RIGHT/UL/UR/DL/DR
		if (Input.GetAxis("DpadHorizontal") != 0.0f) Debug.Log(Input.GetAxis("DpadHorizontal"));
		if (Input.GetAxis("DpadVertical") != 0.0f) Debug.Log(Input.GetAxis("DpadVertical"));
	}

}
