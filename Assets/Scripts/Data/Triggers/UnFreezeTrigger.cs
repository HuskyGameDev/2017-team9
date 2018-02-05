using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnFreezeTrigger : PuzzleComponents.DataTrigger {

	public override void Trigger() {
		Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
		rb.constraints = new RigidbodyConstraints();
		rb.AddForce(Vector3.left * 20.0f);
	}
}
