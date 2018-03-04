using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpriteVisuals : MonoBehaviour {
	public SpriteRenderer center;
	public SpriteRenderer channel;
	public SpriteRenderer inputArrows;
	public SpriteRenderer outputArrows;
	public SpriteRenderer line;
	public SpriteRenderer component;

	public void UpdateVisuals() {
		GridSquare square = gameObject.GetComponent<GridSquare>();

		if (square.type == GridSquare.GridType.Empty) {
			center.sprite = Resources.Load<Sprite>("Sprites/GridPieces/Icon_GridCenter");
			component.sprite = null;
		}
		else {
			center.sprite = Resources.Load<Sprite>("Sprites/GridPieces/Icon_GridComponent");
			component.sprite = Resources.Load<Sprite>("Sprites/Components/Sprite_" + GridSquare.typeToString[(int)square.type]);
		}


		{
			//Create the proper text string for loading a line sprite
			string gridLineText = "Sprites/GridPieces/Icon_GridLine_";
			//0 = No line
			//1 = line
			// we compile a 4 bit segment (EX: 0101) to tell what lines are loaded in Up Right Down Left order
			for (int i = 0; i < square.socketState.Length; i++) {
				gridLineText += (square.socketState[i] == GridSquare.SocketState.None) ? "0" : "1";
			}


			if (gridLineText == "Sprites/GridPieces/Icon_GridLine_0000")
				channel.sprite = null;
			else
				channel.sprite = Resources.Load<Sprite>(gridLineText);
		}


		{ 
			string gridInputArrowText = "Sprites/GridPieces/Icon_GridLine_";
			//0 = No line
			//1 = line
			// we compile a 4 bit segment (EX: 0101) to tell what lines are loaded in Up Right Down Left order
			for (int i = 0; i < square.socketState.Length; i++) {
				gridInputArrowText += (square.socketState[i] == GridSquare.SocketState.Input) ? "1" : "0";
			}


			if (gridInputArrowText == "Sprites/GridPieces/Icon_GridLine_0000")
				inputArrows.sprite = null;
			else
				inputArrows.sprite = Resources.Load<Sprite>(gridInputArrowText);
		}

		{
			string gridOutputArrowText = "Sprites/GridPieces/Icon_GridLine_";
			//0 = No line
			//1 = line
			// we compile a 4 bit segment (EX: 0101) to tell what lines are loaded in Up Right Down Left order
			for (int i = 0; i < square.socketState.Length; i++) {
				gridOutputArrowText += (square.socketState[i] == GridSquare.SocketState.Output) ? "1" : "0";
			}


			if (gridOutputArrowText == "Sprites/GridPieces/Icon_GridLine_0000")
				outputArrows.sprite = null;
			else
				outputArrows.sprite = Resources.Load<Sprite>(gridOutputArrowText);
		}

	}
}
