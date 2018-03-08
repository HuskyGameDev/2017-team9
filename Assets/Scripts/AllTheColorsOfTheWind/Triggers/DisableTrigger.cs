using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class DisableTrigger : ColorTrigger {
	public override void Trigger() {
		this.gameObject.SetActive(false);
	}
}
