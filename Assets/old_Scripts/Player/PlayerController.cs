using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Camera playerCamera;
	public CursorManager cursor;
	public float speed = 10.0f;
	public float lookSensitivity = 10.0f;
	public bool invertedLook = false;
	public Vector2 verticalLookBounds;
	//private Rigidbody body;
	private Rigidbody character;
	new private CapsuleCollider collider;
	private bool isGrounded;

	//Keep track of where we where last standing
	private GameObject prevGround = null;
	private Vector3 prevGroundPosition = Vector3.zero;

	private float _rotation;
	private float rotation {
		get { return _rotation; }
		set { 
			float t = value;
			if (t < -360.0f)
				t += 360.0f;
			if (t > 360.0f)
				t -= 360.0f;
			_rotation = t;
		}
	}

	private float _camRotation;
	private float camRotation {
		get { return _camRotation; }
		set { 
			float t = value;
			if (t < verticalLookBounds.x)
				t = verticalLookBounds.x;
			if (t > verticalLookBounds.y)
				t = verticalLookBounds.y;
			_camRotation = t;
		}
	}


	public Vector3 DebugVector;
	// Use this for initialization
	void Start () {
		isGrounded = false;
		character = GetComponent<Rigidbody>();
		collider = GetComponent<CapsuleCollider>();
		//body = GetComponent<Rigidbody>(); 
	}


	void Update() {
		cursor.Switch(cursor.defaultCursor);
		Cursor.lockState = CursorLockMode.Locked;

		RaycastHit clickInfo;
		Physics.SphereCast(playerCamera.transform.position, .2f, playerCamera.transform.forward, out clickInfo, 1.5f);
		Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1.5f);

		if (clickInfo.transform != null && clickInfo.transform.gameObject.GetComponents<Interactable>().Length > 0) {
			cursor.Switch(cursor.overCursor);
			if (Input.GetButtonDown("Fire1")) {
				foreach (Interactable i in clickInfo.transform.gameObject.GetComponents<Interactable>()) {
					i.Interact();
				}
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		character.velocity = Vector3.zero;
		float timeDelta = Time.fixedDeltaTime;
		//body.velocity = new Vector3(0.0f,body.velocity.y,0.0f);
		//Cursor.lockState = CursorLockMode.Locked;
		Vector2 movementInput = getMovementInput();
		Vector3 movementVector = ((this.transform.forward * movementInput.y) + (this.transform.right * movementInput.x)).normalized;

		//Do a ray cast to find the ground.
		RaycastHit footInfo;
		Physics.SphereCast(transform.position, collider.radius, Vector3.down, out footInfo, collider.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

		RaycastHit headInfo;
		Physics.SphereCast(transform.position, float.MaxValue, Vector3.down, out headInfo, collider.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
		Debug.DrawRay(transform.position, Vector3.down * 10000.0f, Color.green);
		Debug.DrawRay(transform.position, Vector3.down * (collider.radius + (collider.height/2f)), Color.blue);

		//Normalize the movement vector to the ground, this helps up follow slopes
		movementVector = Vector3.ProjectOnPlane(movementVector, footInfo.normal).normalized;

		movementVector.x = movementVector.x * speed * timeDelta;
		movementVector.z = movementVector.z * speed * timeDelta;


			//Follow the platform if we are standing on it!
		if (footInfo.transform != null) {
			Debug.Log("Ground!");
			if (isGrounded) {
				if (prevGround != null && footInfo.transform.gameObject == prevGround) {
					movementVector += footInfo.transform.gameObject.transform.position - prevGroundPosition;
				}
				//Keep track of the last grounSSd
				prevGround = footInfo.transform.gameObject;
				prevGround.GetComponent<Renderer>().material.color = Color.white;
				prevGroundPosition = footInfo.transform.gameObject.transform.position;
			}
			isGrounded = true;
			//movementVector.y -= 0.05f;
		}
        else{
			Debug.Log("NOGround");
			//If we are not grounded, we have no ground to store
			if (prevGround != null)prevGround.GetComponent<Renderer>().material.color = Color.black;
			prevGround = null;
			isGrounded = false;
			//And we apply gravity.
			//finalMovementVector += Vector3.down * 20.0f * timeDelta;
		}
		Debug.DrawRay(this.transform.position, movementVector);
		DebugVector = movementVector;
		character.MovePosition(this.transform.position + movementVector);
		ManageRotation();
	}

	private void ManageRotation() {
		Vector2 lookInput = getLookInput();
		camRotation += lookInput.y * lookSensitivity * Time.fixedDeltaTime * ((invertedLook) ? 1.0f : -1.0f);
		playerCamera.transform.localEulerAngles = new Vector3(camRotation, 0.0f, 0.0f);
		transform.Rotate(0.0f,lookInput.x * lookSensitivity * Time.fixedDeltaTime, 0.0f);
	}

	private Vector2 getMovementInput() {
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private Vector2 getLookInput() {
		return new Vector2(Input.GetAxis("LookHorizontal") + Input.GetAxis("Mouse X"), Input.GetAxis("LookVertical") + Input.GetAxis("Mouse Y"));
	}


}
