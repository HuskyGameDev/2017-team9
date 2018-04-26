using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class PuzzleCompleteTrigger : ColorTrigger {
	public override void Trigger() {
		AkSoundEngine.PostEvent("PuzzleComplete", PlayerControls.instance.gameObject);
	}

	public override void Untrigger() {
	}
}
