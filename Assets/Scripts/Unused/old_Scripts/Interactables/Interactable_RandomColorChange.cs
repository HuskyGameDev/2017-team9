using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_RandomColorChange : Interactable {
	public override void Interact() {
		this.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
	}
}
