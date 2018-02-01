using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : PuzzleComponents.DataTrigger {
	public override void Trigger() {
		Debug.Log("Quit");
		Application.Quit();
	}
}
