using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CursorManager {
	public Image cursor;
	public Sprite defaultCursor;
	public Sprite overCursor;

	public void Switch(Sprite newSprite) {
		cursor.sprite = newSprite;
	}
}
