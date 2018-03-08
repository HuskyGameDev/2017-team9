using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class Receptacle : Interactable {

	public Vector3 heldObjectPosition = new Vector3(0, 0.2f, 0);
	public Quaternion heldObjectRotation = new Quaternion();
	public GameObject heldObject = null;
	public ColorTrigger[] triggers;

	public override void Interact() {
		//Debug.Log("Interact called on: " + this.name);

		GameObject playerGameObject = PlayerControls.instance.gameObject;
		if (heldObject == null) {	// can't accept a new object if already holding one
			if (playerGameObject.GetComponent<PlayerControls>().heldObject != null) {	// can't accept a non-existant object
				heldObject = playerGameObject.GetComponent<PlayerControls>().heldObject;	// get refference to object
				playerGameObject.GetComponent<PlayerControls>().heldObject = null;	// reset players refference to object

				//Debug.Log("Set heldObject: " + heldObject.name);

				heldObject.transform.parent = this.gameObject.transform;	// set object as child of recepticle
				heldObject.transform.localPosition = heldObjectPosition;	// set object's position and rotation relative to recepticle
				heldObject.transform.localRotation = heldObjectRotation;

				/*foreach (ColorTrigger t in triggers) {
					Debug.Log("testing trigger with data: "+ heldObject.GetComponent<Pickupable>().GetDataSequence().GetStringRepresentation());
					t.DataChange(heldObject.GetComponent<Pickupable>().GetDataSequence());
				}*/
			}
		}
	}
}
