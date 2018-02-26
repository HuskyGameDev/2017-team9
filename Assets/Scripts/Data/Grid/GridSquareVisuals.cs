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
	public Texture encoderTexture;
	public Texture mixerTexture;

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



<<<<<<< HEAD
	public void EnableComponent(GridSquare.GridType type) {
		component.SetActive(true);
		if (type == GridSquare.GridType.Empty)
			component.SetActive(false);
		else if (type == GridSquare.GridType.Unusable)
			SetTexture(unusableTexture);
		else if (type == GridSquare.GridType.Source)
			SetTexture(sourceTexture);
		else if (type == GridSquare.GridType.Shifter)
			SetTexture(shifterTexture);
		else if (type == GridSquare.GridType.Linker)
			SetTexture(linkerTexture);
		else if (type == GridSquare.GridType.Deleter)
			SetTexture(deleterTexture);
		else if (type == GridSquare.GridType.Connector)
			SetTexture(connectorTexture);
		else if (type == GridSquare.GridType.Combiner)
			SetTexture(combinerTexture);
		else if (type == GridSquare.GridType.Adder)
			SetTexture(adderTexture);
		else if (type == GridSquare.GridType.Encoder)
			SetTexture(encoderTexture);
		else if (type == GridSquare.GridType.Mixer)
			SetTexture(mixerTexture);
	}


	private void SetTexture(Texture t) {
		component.GetComponent<Renderer>().material.SetTexture("_MainTex", t);
=======
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
>>>>>>> 6eecd56deaee774108668ff0ac51598e1894e0e4
	}
}
