using UnityEngine;
using UnityEngine.UI;
using PuzzleComponents;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour {

	public GameObject playerCamera;
	public float moveSpeed = 10.0f;
	public float rotationSpeed = 1.0f;
	public float pitchSpeed = 1.0f;
	public Vector2 pitchBounds = new Vector2(-45,45);
	public LayerMask ignoreMask;
	public CursorManager cursor;
	public UIManager PlayerUI;

	private float internalRotation = 0.0f;
	private float cameraPitch = 0.0f;

	private CharacterController body;

	private PuzzleComponents.DataPoint connectionBuffer;


	void Awake () {
		//Debug.Log(this.gameObject.transform.rotation.y + "|" + this.gameObject.transform.localRotation.y);
		this.internalRotation = this.gameObject.transform.rotation.y;
		body = this.gameObject.GetComponent<CharacterController>();
	}

	private void FixedUpdate() {
		handleCamera();
		handleMovement();
	}
	void Update () {
		cursor.Switch(cursor.defaultCursor);
		Cursor.lockState = CursorLockMode.Locked;

		RaycastHit rayInfo;
		Physics.SphereCast(playerCamera.transform.position, .1f, playerCamera.transform.forward, out rayInfo, 2.0f, ignoreMask);
		Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1.5f);

		PlayerUI.type.text = "";
		PlayerUI.output.text = "";
		PlayerUI.input.text = "";
		PlayerUI.trigger.text = "";

		if (rayInfo.transform != null) {

			//The following section was made not necessary when we switched away from the DataPoint connection scheme. It can be removed at a future date but for now exists for reference
			/*
			//Check if we are over a data point
			if (rayInfo.transform.gameObject.tag == "DataPoint") {
				//If we are this means we can interact so change the cursor
				cursor.Switch(cursor.overCursor);
				DataPoint t = rayInfo.transform.gameObject.GetComponent<PuzzleComponents.DataPoint>();
				PlayerUI.type.text = t.owner.GetString();
				if (t.owner.output.Length > 0)
					PlayerUI.output.text = t.owner.GetOutputString();

				if ((t.owner.input.Length > 0 && t.owner.input[0] != null && t.owner.input[0].IsConnected() != false && t.owner.input[0].partner.owner.GetOutput() != null)) {
					PlayerUI.input.text = t.owner.input[0].partner.owner.GetOutputString();
				}

				//Create all the trigger panels
				for (int k = 0; k < t.owner.triggers.Length; k++) {
					PlayerUI.trigger.text = PlayerUI.trigger.text + "\n Trigger: " + ((new DataSequence(t.owner.triggers[k].triggerData)).GetStringRepresentation());
				}

				//Check if we want to interact with the data point
				if (InputManager.GetGameButtonDown(InputManager.GameButton.Interact1)) {
					//Get the data point of our target

					//Store it if we are not storing one already
					if (connectionBuffer == null) {
						connectionBuffer = t;
					}
					else {
						//Otherwise create the connection and clear the connectionBuffer
						t.CreateConnection(connectionBuffer);
						connectionBuffer = null;
					}
				}
			}*/


		}

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

		TerminalInteraction();
	}

	private GameObject last;

	private void TerminalInteraction() {
		RaycastHit rayInfo;
		Physics.Raycast(playerCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width * 0.5f , Screen.height * 0.5f)), out rayInfo, ignoreMask);
		if (rayInfo.transform != null && rayInfo.transform.gameObject.tag == "DataTerminal") {
			Vector3 surfacePoint = rayInfo.point - rayInfo.collider.transform.position;
			Debug.Log(surfacePoint.y);
			Debug.Log(rayInfo.textureCoord2);
			Debug.Log(" " + Screen.width * rayInfo.textureCoord2.x + " " + Screen.height * rayInfo.textureCoord2.y);


			Camera terminalCam = rayInfo.transform.gameObject.GetComponent<DataTerminal>().cam;

			Vector2 guess = new Vector2(terminalCam.pixelHeight * rayInfo.textureCoord2.x, terminalCam.pixelHeight * rayInfo.textureCoord2.y);

			if (last != null)
				last.GetComponent<MeshRenderer>().materials[0].color = Color.white;

			RaycastHit rayInfor2;
			Physics.Raycast(terminalCam.ScreenPointToRay(guess), out rayInfor2);
			if (rayInfor2.collider != null) {
				Debug.Log(rayInfor2.collider.gameObject.transform.name);
				last = rayInfor2.collider.gameObject;
				last.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
			}
			else {
				last.GetComponent<MeshRenderer>().materials[0].color = Color.white;
				last = null;
			}

		}

		//Debug.Log(Input.mousePosition);
		//
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
