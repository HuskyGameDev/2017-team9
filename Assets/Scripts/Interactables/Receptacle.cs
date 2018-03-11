﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class Receptacle : Interactable {

	public Vector3 heldObjectPosition = new Vector3(0, 0.2f, 0);
	public Quaternion heldObjectRotation = new Quaternion();
	public GameObject heldObject = null;	// reference to held object
	public Sprite outSideSprite;   // sprite to use when generating visuals
	public Sprite inSideSprite; // another sprite to use when generating visuals
	public bool lockInPickupable = false;   // if this pedastal locks in pickupables that trigger something
	public ColorTrigger[] triggers; // triggers attached to this pedestal

	private ColorBit[] triggerColors;	// used with visuals, if one of the colors supplied by a pickupable matches one of these, makes the signal object visisble
	private GameObject[,] triggerVisuals;	// these objects are made active if a pickupable supplies the matching color

	private void Awake() {  // move held object to place it should be
		moveHeldObject();
		GenerateVisuals();
	}

	/// <summary>
	/// Finds all of the colors for attached triggers, and returns materials using those colors for generating visuals
	/// </summary>
	/// <returns>materials using those colors for generating visuals</returns>
	private Material[] FindTriggerColors() {
		if (triggers.Length < 1) {
			triggerColors = new ColorBit[0];
			return new Material[0];
		}

		Material[] temp = new Material[triggers.Length];
		ColorBit[] colorTemp = new ColorBit[triggers.Length];
		int size = 1;
		temp[0] = new Material(Shader.Find("Sprites/Diffuse"));
		temp[0].SetColor("_Color", triggers[0].triggerColor.color);
		colorTemp[0] = triggers[0].triggerColor;

		for (int i = 1; i < triggers.Length; i++) {
			bool exists = false;
			for (int j = 0; j < size; j++) {
				if (temp[j].color.Equals(triggers[i].triggerColor.color)) {
					exists = true;
				}
			}
			if (!exists) {
				temp[size] = new Material(Shader.Find("Sprites/Diffuse"));
				temp[size].SetColor("_Color", triggers[i].triggerColor.color);
				colorTemp[size] = triggers[i].triggerColor;
				size += 1;
			}
		}

		Material[] triggerMaterials = new Material[size];
		triggerColors = new ColorBit[size];
		for (int i = 0; i < size; i++) {
			triggerMaterials[i] = temp[i];
			triggerColors[i] = colorTemp[i];
		}
		return triggerMaterials;
	}
	
	/// <summary>
	/// Generates visuals
	/// </summary>
	public void GenerateVisuals() {
		if (outSideSprite == null) {   // load sprite for use with SpriteRenderer
			outSideSprite = Resources.Load<Sprite>("Sprites/Interactible/OutSides");
		}
		if (inSideSprite == null) {   // load sprite for use with SpriteRenderer
			inSideSprite = Resources.Load<Sprite>("Sprites/Interactible/InSides");
		}

		for (int i = this.transform.childCount - 1; i >= 0; i--) {      // remove existing visuals
			if (this.transform.GetChild(i).gameObject.name == "Side") {
				DestroyImmediate(this.transform.GetChild(i).gameObject, true);
			}
		}
		Material[] triggerMaterials = FindTriggerColors();
		triggerVisuals = new GameObject[triggerColors.Length, 4];

		if (triggers.Length > 0) {
			GameObject[] visual = new GameObject[4];
			for (int i = 0; i < 4; i++) {
				visual[i] = GenerateSide(triggerMaterials, i);
				visual[i].transform.parent = this.transform;
				visual[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			}

			visual[0].transform.localPosition = new Vector3(0.501f, 0, 0);
			visual[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
			visual[1].transform.localPosition = new Vector3(0, 0, 0.501f);
			visual[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
			visual[2].transform.localPosition = new Vector3(-0.501f, 0, 0);
			visual[2].transform.localRotation = Quaternion.Euler(0, -90, 0);
			visual[3].transform.localPosition = new Vector3(0, 0, -0.501f);
			visual[3].transform.localRotation = Quaternion.Euler(0, 180, 0);
		}

		updateVisuals();
	}

	/// <summary>
	/// Generate a group of sprites with one of each color that this pickupble provides
	/// </summary>
	/// <returns></returns>
	private GameObject GenerateSide(Material[] triggerMaterials, int k) {
		GameObject visual = new GameObject("Side");
		float width = 1.0f / triggerMaterials.Length;

		for (int i = 0; i < triggerMaterials.Length; i++) {
			GameObject planeSec = new GameObject("Section");
			planeSec.AddComponent<SpriteRenderer>();
			//planeSec.layer = 2;	// puts section on Ingnore-Raycast layer
			planeSec.transform.parent = visual.transform;
			planeSec.transform.localScale = new Vector3(width, 1, 1);
			planeSec.transform.localPosition = new Vector3(-0.5f + (width * (i + 0.5f)), 0, 0);
			planeSec.GetComponent<SpriteRenderer>().material = triggerMaterials[i];
			planeSec.GetComponent<SpriteRenderer>().sprite = outSideSprite;

			//Debug.Log("Creating triggerVisuals[" + i + ", " + k + "]");
			triggerVisuals[i, k] = new GameObject("TriggerVisual");
			triggerVisuals[i, k].AddComponent<SpriteRenderer>();
			triggerVisuals[i, k].transform.parent = planeSec.transform;
			triggerVisuals[i, k].transform.localScale = new Vector3(1, 1, 1);
			triggerVisuals[i, k].transform.localPosition = new Vector3(0, 0, 0);
			triggerVisuals[i, k].GetComponent<SpriteRenderer>().color = triggerMaterials[i].color;
			triggerVisuals[i, k].GetComponent<SpriteRenderer>().sprite = inSideSprite;
			
		}

		return visual;
	}
	
	/// <summary>
	/// 
	/// </summary>
	public override void Interact() {
		//Debug.Log("Interact called on: " + this.name);

		GameObject playerGameObject = PlayerControls.instance.gameObject;
		if (heldObject == null) {   // can't accept a new object if already holding one
			if (playerGameObject.GetComponent<PlayerControls>().heldObject != null) {   // can't accept a non-existant object
				heldObject = playerGameObject.GetComponent<PlayerControls>().heldObject;    // get refference to object
				playerGameObject.GetComponent<PlayerControls>().heldObject = null;  // reset players refference to object

				//Debug.Log("Set heldObject: " + heldObject.name);

				moveHeldObject();

				//Debug.Log("Calling updateVisuals");
				updateVisuals();

				bool triggered = false;
				foreach (ColorBit c in heldObject.GetComponent<Pickupable>().color) {
					foreach (ColorTrigger t in triggers) {
						bool trip = t.Check(c);
						if (trip) {
							triggered = true;
						}
					}
				}

				if (triggered && lockInPickupable) {
					//Debug.Log("Set canPickUp to false");
					heldObject.GetComponent<Pickupable>().canPickUp = false;
				}
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public void updateVisuals() {
		//Debug.Log("Updating Receptacle visuals");
		if (triggerVisuals == null || triggerVisuals.Length == 0) {
			//Debug.Log("triggerVisuals == null: Exiting");
			return;
		}

		if (heldObject == null) {
			//Debug.Log("No held object, turn off visuals");
			foreach (GameObject o in triggerVisuals) {
				//Debug.Log("Turning off visual...");
				o.SetActive(false);
			}
		} else {
			//Debug.Log("Held object exists, turn on visuals");
			foreach (GameObject o in triggerVisuals) {
				//Debug.Log("Turning off visual...");
				o.SetActive(false);
			}

			for (int c = 0; c < heldObject.GetComponent<Pickupable>().color.Length; c++) {  // for each color provided by held object
				for (int t = 0; t < triggerColors.Length; t++) {    // for each color used by a trigger
					if (triggerColors[t].Equals(heldObject.GetComponent<Pickupable>().color[c])) {   // check if provided color matches color used by trigger
						Debug.Log(triggerColors[t].ToString() + " and " + heldObject.GetComponent<Pickupable>().color[c].ToString() + " are equal");
						for (int j = 0; j < 4; j++) {   // if colors do match, switch on all in group
							triggerVisuals[t, j].SetActive(true);
							//Debug.Log("Turning on triggerVisuals[" + t + ", " + j + "]");
						}
						
					}
				}
			}
		}
	} 

	/// <summary>
	/// 
	/// </summary>
	public void moveHeldObject() {
		if (heldObject != null) {
			heldObject.transform.parent = this.gameObject.transform;    // set object as child of recepticle
			heldObject.transform.localPosition = heldObjectPosition;    // set object's position and rotation relative to recepticle
			heldObject.transform.localRotation = heldObjectRotation;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public void removeHeldObject() {
		heldObject = null;
		updateVisuals();
	}
}