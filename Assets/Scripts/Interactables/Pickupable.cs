using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickupable : Interactable {

	public Vector3 heldObjectPosition = new Vector3( -0.2f, 0.2f, 0.2f);

	/*
	private DataSequence source = new DataSequence(new DataSegment[0]);
	public DataSegment[] _source;

	public void Setup() {
		//Debug.Log("Pickupable setup: " + this.name);
		source = new DataSequence(_source);
	}

	public DataSequence GetDataSequence() {
		source = new DataSequence(_source);
		return source;
	}*/

	public override void Interact() {
		GameObject playerGameObject = PlayerControls.instance.gameObject;
		if (playerGameObject.GetComponent<Pickupable>() == null) {
			//if (this.transform.parent.gameObject.tag == "Interactable") {
			if (this.transform.parent.gameObject.GetComponent<Receptacle>() != null) {	// if this object is "in" (a child of) a receptical
				this.transform.parent.gameObject.GetComponent<Receptacle>().heldObject = null;  // reset Receptacle's reference to this object
			}
			//}
			playerGameObject.GetComponent<PlayerControls>().heldObject = this.gameObject;	// set kommrade's heldObject reference to this object
			this.transform.parent = playerGameObject.transform;	// set kommrade as this object's parent
			this.transform.localPosition = heldObjectPosition;  // set this object's position relative to kommrade
		}
	}
}
