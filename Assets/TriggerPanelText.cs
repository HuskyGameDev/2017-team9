using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPanelText : MonoBehaviour {

	public Text text;

	public void SetText(string input) {
		text.text = input;
	}
}
