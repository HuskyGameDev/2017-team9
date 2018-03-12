using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class EnableTrigger : ColorTrigger {
	public override void Trigger() {
		this.gameObject.SetActive(true);
	}
	public override void Untrigger() {
		this.gameObject.SetActive(false);
	}

	public void Awake() {
		this.gameObject.SetActive(false);
	}
}
