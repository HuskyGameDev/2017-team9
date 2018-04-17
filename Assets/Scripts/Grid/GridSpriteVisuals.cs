
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpriteVisuals : MonoBehaviour {
	public SpriteRenderer center;
	public SpriteRenderer channel;
	public SpriteRenderer inputArrows;
	public SpriteRenderer outputArrows;
	public SpriteRenderer[] lines;
	public SpriteRenderer component;

	private static string Anim_InToOut = "Sprites/Grid/Line/InToOut/Anim_InToOut_";
	private static string Anim_OutToIn = "Sprites/Grid/Line/OutToIn/Anim_OutToIn_";


	private float lineAnimSpeed = 5.0f;


	public void UpdateVisuals() {
		GridSquare square = gameObject.GetComponent<GridSquare>();

		if (square.type == GridSquare.GridType.Empty) {
			center.sprite = Resources.Load<Sprite>("Sprites/Grid/Structure/Sprite_Grid_Center");
			component.sprite = null;
		}
		else {
			center.sprite = Resources.Load<Sprite>("Sprites/Grid/Structure/Sprite_Grid_Component");
			component.sprite = Resources.Load<Sprite>("Sprites/Grid/Components/Sprite_Component_" + GridSquare.typeToString[(int)square.type]);
		}


		{
			//Create the proper text string for loading a line sprite
			string gridLineText = "Sprites/Grid/Structure/Sprite_Grid_Channel_";
			//0 = No line
			//1 = line
			// we compile a 4 bit segment (EX: 0101) to tell what lines are loaded in Up Right Down Left order
			for (int i = 0; i < square.socketState.Length; i++) {
				gridLineText += (square.socketState[i] == GridSquare.SocketState.None) ? "0" : "1";
			}


			if (gridLineText == "Sprites/Grid/Structure/Sprite_Grid_Channel_0000")
				channel.sprite = null;
			else
				channel.sprite = Resources.Load<Sprite>(gridLineText);
		}


		{ 
			string gridInputArrowText = "Sprites/Grid/Structure/Sprite_Grid_Channel_";
			//0 = No line
			//1 = line
			// we compile a 4 bit segment (EX: 0101) to tell what lines are loaded in Up Right Down Left order
			for (int i = 0; i < square.socketState.Length; i++) {
				gridInputArrowText += (square.socketState[i] == GridSquare.SocketState.Input) ? "1" : "0";
			}


			if (gridInputArrowText == "Sprites/Grid/Structure/Sprite_Grid_Channel_0000")
				inputArrows.sprite = null;
			else
				inputArrows.sprite = Resources.Load<Sprite>(gridInputArrowText);
		}

		{
			string gridOutputArrowText = "Sprites/Grid/Structure/Sprite_Grid_Channel_";
			//0 = No line
			//1 = line
			// we compile a 4 bit segment (EX: 0101) to tell what lines are loaded in Up Right Down Left order
			for (int i = 0; i < square.socketState.Length; i++) {
				gridOutputArrowText += (square.socketState[i] == GridSquare.SocketState.Output) ? "1" : "0";
			}


			if (gridOutputArrowText == "Sprites/Grid/Structure/Sprite_Grid_Channel_0000")
				outputArrows.sprite = null;
			else
				outputArrows.sprite = Resources.Load<Sprite>(gridOutputArrowText);
		}

	}

	public IEnumerator DrawLineInDirection(GridSquare.GridDirection dir, GridSquare a) {
		GridSquare b = a.neighbors[(int)dir];
		b.sprites.lines[(int)GridSquare.oppositeDirection[(int)dir]].sprite = null;
		yield return a.sprites.StartCoroutine(AnimFromResources(a.sprites.lines[(int)dir], Anim_InToOut, 33, true, lineAnimSpeed));
		yield return b.sprites.StartCoroutine(AnimFromResources(b.sprites.lines[(int)GridSquare.oppositeDirection[(int)dir]], Anim_OutToIn, 33, true, lineAnimSpeed));

		a.EnsureSquareIntegrity();
		b.EnsureSquareIntegrity();
		//b.UpdateLine(GridSquare.oppositeDirection[(int)dir]);
	}


	public IEnumerator RemoveLineInDirection(GridSquare.GridDirection dir, GridSquare a) {
		
		yield return a.sprites.StartCoroutine(AnimFromResources(a.sprites.lines[(int)dir], Anim_OutToIn, 33, false, lineAnimSpeed));
		a.sprites.lines[(int)dir].sprite = null;


		GridSquare b = a.neighbors[(int)dir];

		if (b == null)
			yield break;

		yield return b.sprites.StartCoroutine(AnimFromResources(b.sprites.lines[(int)GridSquare.oppositeDirection[(int)dir]], Anim_InToOut, 33, false, lineAnimSpeed));
		b.sprites.lines[(int)GridSquare.oppositeDirection[(int)dir]].sprite = null;

		a.EnsureSquareIntegrity();
		b.EnsureSquareIntegrity();
	}




	public IEnumerator AnimFromResources(SpriteRenderer target, string basePath, int frames, bool forward, float speed) {
		if (forward) {
			for (float i = 1; i <= frames; i += speed) {
				target.sprite = Resources.Load<Sprite>(basePath + Mathf.RoundToInt(i));
				yield return null;
				target.sprite = Resources.Load<Sprite>(basePath + frames);
			}
		}
		else {
			for (float i = frames; i > 0 ; i -= speed) {
				target.sprite = Resources.Load<Sprite>(basePath + Mathf.RoundToInt(i));
				yield return null;
			}
			target.sprite = Resources.Load<Sprite>(basePath + 0);
		}
	}

}
