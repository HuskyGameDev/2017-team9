using UnityEngine;
using UnityEngine.UI;
using PuzzleComponents;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(TerminalInteraction))]
public class PlayerControls : MonoBehaviour {

	public GameObject playerCamera;
	public float moveSpeed = 10.0f;
	public float rotationSpeed = 1.0f;
	public float pitchSpeed = 1.0f;
	public Vector2 pitchBounds = new Vector2(-45,45);
	public LayerMask ignoreMask;
	public CursorManager cursor;
	public UIManager PlayerUI;

	//The Singleton object for player controls
	public static PlayerControls instance;

	private float internalRotation = 0.0f;
	private float cameraPitch = 0.0f;

	private CharacterController body;

	private TerminalInteraction terminalInteractionController;

	void Awake () {
		//Debug.Log(this.gameObject.transform.rotation.y + "|" + this.gameObject.transform.localRotation.y);
		this.internalRotation = this.gameObject.transform.rotation.y;
		body = this.gameObject.GetComponent<CharacterController>();
		if (instance != null) {
			Debug.Log("You have multiple players!");
			Debug.Break();
		}
		instance = this;
		terminalInteractionController = this.gameObject.GetComponent<TerminalInteraction>();
	}

	private void FixedUpdate() {
		//handleCamera();
		//handleMovement();
	}
	void Update () {
		//cursor.Switch(cursor.defaultCursor);
		//Cursor.lockState = CursorLockMode.Locked;

		/*
		if (Input.GetKeyDown(KeyCode.M)) {
			AkSoundEngine.PostEvent("Door_Close", this.gameObject);
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			AkSoundEngine.PostEvent("Door_Lock", this.gameObject);
		}
		if (InputManager.GetButtonDown(InputManager.Button.Y)) {
			cam.GetComponent<CameraManager>().NextRender();
		}
		 */

		terminalInteractionController.Interact();
	}

	private void handleMovement() {

		//So we have two inputs that are basically an X and a Y component of a vector.
		//We can use y (Up and down) as a measure of how much forward we want to move.
		//We can use x (Left and down) as a measure of how much we want to strafe.
		float x = InputManager.GetAxis(InputManager.Axis.LeftHorizontal);
		float y = InputManager.GetAxis(InputManager.Axis.LeftVertical);

		//Now we must do a quick modifcation so that the sum of the two movement vectors is not greater than 1, this prevents diagnol movemenet being faster.
		float sum = Mathf.Abs(x) + Mathf.Abs(y);

		if (sum >= 1) {
			sum = 1.0f / sum;
			x *= sum;
			y *= sum;
		}

		//Translate this local oriented movement direction into one that makes sense in world coordinates
		Vector3 moveDir = transform.TransformDirection(new Vector3(x, 0.0f, y));


		//Apply a very harsh gravity to keep our player on the ground down slopes.
		moveDir.y -= (200.0f) * Time.deltaTime;

		//Move the body.
		body.Move(moveDir * moveSpeed * Time.deltaTime);
	}

	private void handleCamera() {
		//Camera is handled by rotating our player model left and right
		internalRotation += InputManager.GetAxis(InputManager.Axis.RightHorizontal) * rotationSpeed * Time.deltaTime;
		this.transform.localRotation = Quaternion.Euler(0.0f, internalRotation, 0.0f);


		//Camera is Handled by rotation our camera up and down.
		cameraPitch += InputManager.GetAxis(InputManager.Axis.RightVertical) * pitchSpeed * Time.deltaTime;
		if (cameraPitch < pitchBounds.x) cameraPitch = pitchBounds.x;
		if (cameraPitch > pitchBounds.y) cameraPitch = pitchBounds.y;

		playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0.0f, 0.0f);
	}
}
