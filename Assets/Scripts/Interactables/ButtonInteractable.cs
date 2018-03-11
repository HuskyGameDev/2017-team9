using AllTheColorsOfTheWind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : Interactable {

	public Color offColor = new Color(100,0,0);
	public Color onColor = new Color(0,255,0);
	public Sprite offSprite;
	public Sprite onSprite;
	public bool pressed = false;

	public ColorTrigger[] triggers;

	private SpriteRenderer spriteRend;

	public void Awake() {
		if (this.GetComponent<SpriteRenderer>() == null) {
			GenerateVisual();
		}
	}

	public void GenerateVisual() {
		if (offSprite == null) {
			offSprite = Resources.Load<Sprite>("Sprites/Interactible/PowerSprite");
		}
		if (onSprite == null) {
			onSprite = Resources.Load<Sprite>("Sprites/Interactible/PowerSprite");
		}

		for (int i = this.transform.childCount - 1; i >= 0; i--) {  // remove existing visuals
			if (this.transform.GetChild(i).gameObject.name == "Visual") {
				DestroyImmediate(this.transform.GetChild(i).gameObject, true);
			}
		}

		GameObject visual = new GameObject("Visual");
		spriteRend = visual.AddComponent<SpriteRenderer>();

		visual.transform.parent = this.transform;
		visual.transform.localScale = new Vector3(1, 1, 1);
		visual.transform.localPosition = new Vector3(0.501f , 0, 0);
		visual.transform.localRotation = Quaternion.Euler(0, 90, 0);

		updateVisuals();
	}

	public void updateVisuals() {
		if (pressed) {
			spriteRend.color = onColor;
			spriteRend.sprite = onSprite;
		} else {
			spriteRend.color = offColor;
			spriteRend.sprite = offSprite;
		}
	}

	public override void Interact() {
		if (pressed) {
			pressed = false;
			updateVisuals();
			/*foreach (ColorTrigger t in triggers) {	// untrigger all attached triggers
				t.UnTrigger();
			}*/
		} else {
			pressed = true;
			updateVisuals();
			foreach (ColorTrigger t in triggers) {	// trigger all attached triggers
				t.Trigger();
			}
		}
	}

}
