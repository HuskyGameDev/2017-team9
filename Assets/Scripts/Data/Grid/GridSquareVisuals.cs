using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visuals for a gridSquare
/// </summary>
[RequireComponent(typeof(GridSquare))]
public class GridSquareVisuals : MonoBehaviour {

	//Various textures for the component to use so we can tell them apart.
	public Texture unusableTexture;
	public Texture sourceTexture;
	public Texture shifterTexture;
	public Texture linkerTexture;
	public Texture deleterTexture;
	public Texture connectorTexture;
	public Texture combinerTexture;
	public Texture adderTexture;

	/// <summary>
	/// Checks state of the square and updates visuals
	/// </summary>
	public void UpdateVisuals() {


		GridSquareRegular child = gameObject.GetComponentInChildren<GridSquareRegular>();
		GridSquare square = gameObject.GetComponent<GridSquare>();

		child.upLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Up] != GridSquare.SocketState.None);
		child.downLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Down] != GridSquare.SocketState.None);
		child.leftLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Left] != GridSquare.SocketState.None);
		child.rightLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Right] != GridSquare.SocketState.None);

		if (square.type != GridSquare.GridType.Empty) {
			((GridSquareComponent)child).component.GetComponent<Renderer>().material.SetTexture("_MainTex", getTexture());
		}
	}



	private Texture getTexture() {
		GridSquare square = gameObject.GetComponent<GridSquare>();
		if (square.type == GridSquare.GridType.Unusable)
			return unusableTexture;
		else if (square.type == GridSquare.GridType.Source)
			return sourceTexture;
		else if (square.type == GridSquare.GridType.Shifter)
			return shifterTexture;
		else if (square.type == GridSquare.GridType.Linker)
			return linkerTexture;
		else if (square.type == GridSquare.GridType.Deleter)
			return deleterTexture;
		else if (square.type == GridSquare.GridType.Connector)
			return connectorTexture;
		else if (square.type == GridSquare.GridType.Combiner)
			return combinerTexture;
		else if (square.type == GridSquare.GridType.Adder)
			return adderTexture;
		return null;
	}
}
