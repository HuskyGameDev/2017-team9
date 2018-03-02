using UnityEngine;
using UnityEngine.UI;
using PuzzleComponents;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GridSurfer))]
public class PlayerControls : MonoBehaviour {

	/// <summary>
	/// Freemove - the player is allowed to move around and control the camera at will
	/// Locked - the player is unable to do anything
	/// LockedWtihMouse - the player is unable to move the character or the camera, but is able to freemove the mouse
	/// GridInteraction - the player gameobject is disabled, and the player's input is used to control drawing on the grid
	/// GridInteractionTransition - used to allow for a smooth transition setup.
	/// </summary>
	public enum PlayerState { Freemove, Locked, LockedWithMouse, GridInteraction, GridInteractionTransition}
	public PlayerState state = PlayerState.Freemove;



	public GameObject playerCamera;
	public float moveSpeed = 10.0f;
	public float rotationSpeed = 1.0f;
	public float pitchSpeed = 1.0f;
	public float terminalInteractionDistance = 3.0f;
	public Vector2 pitchBounds = new Vector2(-45,45);
	public LayerMask ignoreMask;
	public CursorManager cursor;
	//public UIManager PlayerUI;

	//The Singleton object for player controls
	public static PlayerControls instance;

	private float internalRotation = 0.0f;
	private float cameraPitch = 0.0f;
	private CharacterController body;

	/// <summary>
	/// A helper script for interacting with terminals
	/// </summary>
	private GridSurfer gridSurfer;

	/// <summary>
	/// Happens whenever this game object is enabled, or the start of the scene
	/// </summary>
	void Awake () {
		//Debug.Log(this.gameObject.transform.rotation.y + "|" + this.gameObject.transform.localRotation.y);
		this.internalRotation = this.gameObject.transform.rotation.y;
		body = this.gameObject.GetComponent<CharacterController>();
		if (instance != null) {
			Debug.Log("You have multiple players!");
			Debug.Break();
		}
		instance = this;
		//terminalInteractionController = this.gameObject.GetComponent<TerminalInteraction>();
		gridSurfer = this.gameObject.GetComponent<GridSurfer>();

		cursor.Switch(cursor.defaultCursor);
		Cursor.lockState = CursorLockMode.Locked;
	}


	/// <summary>
	/// Returns the pitcj of the camera
	/// </summary>
	/// <returns></returns>
	public float GetCamPitch() {
		return cameraPitch;
	}

	/// <summary>
	/// Called every physics step
	/// </summary>
	private void FixedUpdate() {
		if (state == PlayerState.Freemove) {
			handleMovement();
		}
	}

	/// <summary>
	/// Happens after all other scrips (except for other LateUpdate functions)
	/// </summary>
	private void LateUpdate() {
		if (state == PlayerState.Freemove) {
			handleCamera();
		}
	}

	/// <summary>
	/// Called once a frame, used for handling player action.
	/// </summary>
	void Update () {

		//This section allows the player to lock the camera and look around with the mouse
		if (state == PlayerState.Freemove && InputManager.GetGameButton(InputManager.GameButton.CameraLock))
			state = PlayerState.Freemove;// PlayerState.LockedWithMouse;
		else if (state == PlayerState.LockedWithMouse && InputManager.GetGameButton(InputManager.GameButton.CameraLock) == false)
			state = PlayerState.Freemove;

		//Change over the cursor
		if (state == PlayerState.LockedWithMouse) {
			cursor.Switch(cursor.overCursor);
			Cursor.lockState = CursorLockMode.None;
		}
		else {
			cursor.Switch(cursor.defaultCursor);
			Cursor.lockState = CursorLockMode.Locked;
		}


		//Check if we need to switch to gridinteraction mode
		if (state == PlayerState.LockedWithMouse) {
			//If the player is in the right state, and they have clicked
			if (InputManager.GetGameButtonDown(InputManager.GameButton.Interact1)) {
				RaycastHit rayInfo;
				//Ray, ray info out, max distance, ignoremask

				Physics.Raycast(PlayerControls.instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out rayInfo, terminalInteractionDistance, ignoreMask);
				if (rayInfo.collider != null && rayInfo.collider.gameObject.tag == "GridSquare") {
					Debug.Log("Raycast on: " + rayInfo.collider.gameObject.name);

					//Check if the think we are clicking on is part of a square.
					//[TODO] Make this so that you can continue a half drawn line
					GridSquare square = rayInfo.collider.gameObject.GetComponent<GridSquare>();
					if (square != null && square.type != GridSquare.GridType.Empty) {
						//If this puzzle is ok to edit
						if (square.puzzle.editable) {
							state = PlayerState.GridInteractionTransition;
							gridSurfer.currentSquare = square;
							gridSurfer.StartCoroutine("TransitionToGrid");
						}
					}
				}
			}
		}



		if (state == PlayerState.Freemove && InputManager.GetGameButtonDown(InputManager.GameButton.Interact1)) {
			RaycastHit rayInfo;
			//Ray, ray info out, max distance, ignoremask

			Physics.Raycast(PlayerControls.instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)), out rayInfo, terminalInteractionDistance, ignoreMask);
			if (rayInfo.collider != null && rayInfo.collider.gameObject.tag == "GridSquare") {
				Debug.Log("Raycast on: " + rayInfo.collider.gameObject.name);

				//Check if the think we are clicking on is part of a square.
				GridSquare square = rayInfo.collider.gameObject.GetComponent<GridSquare>();
				if (square != null) {
					//If this puzzle is ok to edit
					if (square.puzzle.editable) {
						state = PlayerState.GridInteractionTransition;
						gridSurfer.currentSquare = square;
						gridSurfer.StartCoroutine("TransitionToGrid");
					}
				}
			}
		}





		//Check if we need to switch out of gridinteraction mode
		if (state == PlayerState.GridInteraction) {
			if (InputManager.GetGameButtonDown(InputManager.GameButton.Cancel) && gridSurfer.state != GridSurfer.GridSurferState.Disabled) {
				state = PlayerState.GridInteractionTransition;
				gridSurfer.currentSquare = null;
				gridSurfer.StartCoroutine("TransitionToPlayer");
			}
		}

	}

	/// <summary>
	/// Handles character movement based on player input
	/// </summary>
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

	/// <summary>
	/// Handles camera orientation change based on player input
	/// </summary>
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
