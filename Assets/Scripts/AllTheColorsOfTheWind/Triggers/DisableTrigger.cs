using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class DisableTrigger : ColorTrigger {
	public override void Trigger() {
		triggered = true;
		this.gameObject.SetActive(false);
	}
	public override void Untrigger() {
		triggered = false;
		this.gameObject.SetActive(true);
	}
}
