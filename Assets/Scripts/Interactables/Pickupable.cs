using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickupable : Interactable {

	public Vector3 heldObjectPosition = new Vector3(-0.2f, 0.2f, 0.2f);
	public Sprite sideSprite;
	public ColorBit[] color;   // color or colors that this pickuipable provides
	public bool canPickUp = true;   // if this can currently be picked up

	/*
	public void Awake() {
	}
	*/

	/// <summary>
	/// Generate 6 sides and move them to appropriates places.
	/// Removes exiting sides if there are any.
	/// </summary>
	public void GenerateVisuals() {
		if (sideSprite == null) {       // load sprite for use with SpriteRenderer
			sideSprite = Resources.Load<Sprite>("Sprites/Interactible/Square");
		}

		for (int i = this.transform.childCount - 1; i >= 0; i--) {	// remove existing visuals
			if (this.transform.GetChild(i).gameObject.name == "Side") {
				DestroyImmediate(this.transform.GetChild(i).gameObject, true);
			}
		}

		if (color.Length > 0) {
			GameObject[] visual = new GameObject[6];
			for (int i = 0; i < 6; i++) {
				visual[i] = GenerateSide();//materials);
				visual[i].transform.parent = this.transform;
				visual[i].transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
			}
			visual[0].transform.localPosition = new Vector3(-0.051f, 0.501f, 0);
			visual[0].transform.localRotation = Quaternion.Euler(90, 0, -90);
			visual[1].transform.localPosition = new Vector3(0.501f, 0, -0.051f);
			visual[1].transform.localRotation = Quaternion.Euler(0, -90, -90);
			visual[2].transform.localPosition = new Vector3(0, -0.051f, 0.501f);
			visual[2].transform.localRotation = Quaternion.Euler(0, 180, 0);
			visual[3].transform.localPosition = new Vector3(0, -0.501f, 0.051f);
			visual[3].transform.localRotation = Quaternion.Euler(-90, 180, 0);
			visual[4].transform.localPosition = new Vector3(-0.501f, 0.051f, 0);
			visual[4].transform.localRotation = Quaternion.Euler(0, 90, 0);
			visual[5].transform.localPosition = new Vector3(0.051f, 0, -0.501f);
			visual[5].transform.localRotation = Quaternion.Euler(0, 0, -90);
			
		}
	}

	/// <summary>
	/// Generate a group of prites with one of each color that this pickupble provides
	/// </summary>
	/// <returns></returns>
	private GameObject GenerateSide() {//Material[] materials) {
		GameObject visual = new GameObject("Side");
		float width = 1.0f / color.Length; //materials.Length;

		for (int i = 0; i < color.Length; i++) { //materials.Length; i++) {
			GameObject planeSec = new GameObject("Section");
			planeSec.AddComponent<SpriteRenderer>();

			//planeSec.layer = 2;
			planeSec.transform.parent = visual.transform;
			planeSec.transform.localScale = new Vector3(width, 1, 1);
			planeSec.transform.localPosition = new Vector3(-0.5f + (width * (0.5f + i)), 0, 0);
			//planeSec.GetComponent<SpriteRenderer>().material = materials[i];
			planeSec.GetComponent<SpriteRenderer>().color = color[i].color;
			planeSec.GetComponent<SpriteRenderer>().sprite = sideSprite;
		}

		return visual;
	}

	public override void Interact() {
		if (!canPickUp) { return; }
		GameObject playerGameObject = PlayerControls.instance.gameObject;
		if (playerGameObject.GetComponent<Pickupable>() == null) {
			if (this.transform.parent.gameObject.GetComponent<Receptacle>() != null) {	// if this object is "in" (a child of) a receptical
				//this.transform.parent.gameObject.GetComponent<Receptacle>().heldObject = null;  // reset Receptacle's reference to this object
				this.transform.parent.gameObject.GetComponent<Receptacle>().removeHeldObject();
			}
			
			playerGameObject.GetComponent<PlayerControls>().heldObject = this.gameObject;	// set kommrade's heldObject reference to this object
			this.transform.parent = playerGameObject.transform;	// set kommrade as this object's parent
			this.transform.localPosition = heldObjectPosition;  // set this object's position relative to kommrade
		}
	}
}
