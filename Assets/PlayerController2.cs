using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

	public float moveSpeed = 6; // move speed
	private float turnSpeed = 90; // turning speed (degrees/second)
	private float lerpSpeed = 10; // smoothing speed
	private float gravity = 10; // gravity acceleration
	public bool isGrounded;
	public float deltaGround = 0.3f; // character is grounded up to this distance
	private float jumpSpeed = 10; // vertical jump initial speed
	private float jumpRange = 10; // range to detect target wall

	private Vector3 surfaceNormal; // current surface normal
	private Vector3 myNormal; // character normal

	private float halfCharacterHeight; // distance from character position to ground
	private bool jumping = false; // flag &quot;I'm jumping to wall&quot;
	private float vertSpeed = 0; // vertical jump current speed

	private Transform myTransform;
	new private Rigidbody rigidbody;

	new public Collider collider; // drag BoxCollider ref in editor

	private void Start() {
		rigidbody = this.gameObject.GetComponent<Rigidbody>();
		collider = this.gameObject.GetComponent<Collider>();
		
		myNormal = transform.up; // normal starts as character up direction
		myTransform = transform;
		rigidbody.freezeRotation = true; // disable physics rotation
										 // distance from transform.position to ground
		halfCharacterHeight = collider.bounds.extents.y / 2.0f;

	}

	private void FixedUpdate() {
		// apply constant weight force according to character normal:
		UpdateNormal();
		rigidbody.AddForce(-gravity * rigidbody.mass * myNormal);
	}

	private void Update() {
		// jump code - jump to wall or simple jump
		if (jumping) return; // abort Update while jumping to a wall

		Ray ray;
		RaycastHit hit;

		if (Input.GetKeyDown(KeyCode.Space)) { // jump pressed:
			ray = new Ray(myTransform.position, myTransform.forward);
			if (Physics.Raycast(ray, out hit, jumpRange)) { // wall ahead?
				JumpToWall(hit.point, hit.normal); // yes: jump to the wall
			}
			else if (isGrounded) { // no: if grounded, jump up
				Debug.Log("Jumpy");
				rigidbody.velocity += jumpSpeed * myNormal;
			}
		}
		UpdateNormal();
		UpdateMovement();
	}

	private void UpdateMovement() {

		// movement code - turn left/right with Horizontal axis:
		myTransform.Rotate(0, InputManager.GetAxis(InputManager.Axis.LeftHorizontal) * turnSpeed * Time.deltaTime, 0);
		// update surface normal and isGrounded:

		// find forward direction with new myNormal:
		Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
		// align character to the new myNormal while keeping the forward direction:
		Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed * Time.deltaTime);
		// move the character forth/back with Vertical axis:
		myTransform.Translate(0, 0, InputManager.GetAxis(InputManager.Axis.LeftVertical) * moveSpeed * Time.deltaTime);
	}


	private void UpdateNormal() {

		// cast ray downwards

		Ray ray;
		RaycastHit hit;
		ray = new Ray(myTransform.position, -myNormal);
		if (Physics.Raycast(ray, out hit)) { 
			// use it to update myNormal and isGrounded
			isGrounded = (hit.distance <= (halfCharacterHeight + deltaGround));
			surfaceNormal = hit.normal;
		}
		else {
			isGrounded = false;
			// assume usual ground normal to avoid "falling forever"
			//surfaceNormal = Vector3.up;
		}
		myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
	} 

	private void JumpToWall(Vector3 point, Vector3 normal) {
		// jump to wall
		jumping = true; // signal it's jumping to wall
		rigidbody.isKinematic = true; // disable physics while jumping
		Vector3 orgPos = myTransform.position;
		Quaternion orgRot = myTransform.rotation;
		Vector3 dstPos = point + (normal * (halfCharacterHeight + 0.5f)); // will jump to 0.5 above wall
		Vector3 myForward = Vector3.Cross(myTransform.right, normal);
		Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

		Debug.DrawRay(point, normal, Color.black, 100.0f);
		Debug.Log(halfCharacterHeight);
		//Debug.DrawLine(point, dstPos, Color.red, 100.0f);
		//Debug.DrawLine(orgPos, dstPos, Color.white, 100.0f);

		StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
		//jumptime
	}

	private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal) {
		for (float t = 0.0f; t < 1.0f;) {
			t += Time.deltaTime;
			myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
			myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
			yield return null; // return here next frame
		}
		myNormal = normal; // update myNormal
		rigidbody.isKinematic = false; // enable physics
		jumping = false; // jumping to wall finished

	}

}