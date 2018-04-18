using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class EnableTrigger : ColorTrigger {
	public override void Trigger() {
		Debug.Log("EnableTrigger Called: " + triggered);
		triggered = true;
		this.gameObject.SetActive(true);
	}
	public override void Untrigger() {
		Debug.Log("EnableUntrigger Called: " + triggered);
		triggered = false;
		this.gameObject.SetActive(false);
	}

	
	public void Awake() {
		if (!triggered) {
			this.gameObject.SetActive(false);
		}
	}
}
