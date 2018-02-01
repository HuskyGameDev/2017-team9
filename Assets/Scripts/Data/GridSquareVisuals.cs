using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visuals for a gridSquare
/// </summary>
[RequireComponent(typeof(GridSquare))]
public class GridSquareVisuals : MonoBehaviour {

	//The individual line visual gameobjects
	public GameObject upLine;
	public GameObject rightLine;
	public GameObject downLine;
	public GameObject leftLine;

	//The individual socket visual gameobjects
	public GameObject upInputSocket;
	public GameObject rightInputSocket;
	public GameObject downInputSocket;
	public GameObject leftInputSocket;
	public GameObject upOutputSocket;
	public GameObject rightOutputSocket;
	public GameObject downOutputSocket;
	public GameObject leftOutputSocket;

	//the component game object
	public GameObject component;

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
		//Disable all so we do not have to do that individually (We may want to consider changing this for performance reasons)
		DisableAll();

		//Store the square so we do not have to call GetComponent repeatedly 
		GridSquare square = this.gameObject.GetComponent<GridSquare>();

		//check the lines
		for (int i = 0; i < square.line.Length; i++) {
			if (square.line[i] != null)
				EnableLine((GridSquare.GridDirection)i);
		}

		//Check the sockets
		for (int i = 0; i < square.socketState.Length; i++) {
			if (square.socketState[i] != GridSquare.SocketState.None)
				EnableSocket((GridSquare.GridDirection)i, square.socketState[i]);
		}

		//Set the component (counts for it being empty)
		EnableComponent(square.type);
	}


	/// <summary>
	/// Disables all visuals so we do not have to worry about things getting left on
	/// </summary>
	public void DisableAll() {
		upLine.SetActive(false);
		rightLine.SetActive(false);
		downLine.SetActive(false);
		leftLine.SetActive(false);

		upInputSocket.SetActive(false);
		rightInputSocket.SetActive(false);
		downInputSocket.SetActive(false);
		leftInputSocket.SetActive(false);

		upOutputSocket.SetActive(false);
		rightOutputSocket.SetActive(false);
		downOutputSocket.SetActive(false);
		leftOutputSocket.SetActive(false);

		component.SetActive(false);
	}

	/// <summary>
	/// Enables the socket gameObject for visuals
	/// </summary>
	/// <param name="dir"></param>
	public void EnableSocket(GridSquare.GridDirection dir, GridSquare.SocketState state) {
		if (state == GridSquare.SocketState.Input) {
			if (dir == GridSquare.GridDirection.Up)
				upInputSocket.SetActive(true);
			else if (dir == GridSquare.GridDirection.Right)
				rightInputSocket.SetActive(true);
			else if (dir == GridSquare.GridDirection.Down)
				downInputSocket.SetActive(true);
			else if (dir == GridSquare.GridDirection.Left)
				leftInputSocket.SetActive(true);
		}
		else if (state == GridSquare.SocketState.Output) {
			if (dir == GridSquare.GridDirection.Up)
				upOutputSocket.SetActive(true);
			else if (dir == GridSquare.GridDirection.Right)
				rightOutputSocket.SetActive(true);
			else if (dir == GridSquare.GridDirection.Down)
				downOutputSocket.SetActive(true);
			else if (dir == GridSquare.GridDirection.Left)
				leftOutputSocket.SetActive(true);
		}
	}

	/// <summary>
	/// Enables the Line gameObject for visuals
	/// </summary>
	/// <param name="dir"></param>
	public void EnableLine(GridSquare.GridDirection dir) {
		if (dir == GridSquare.GridDirection.Up)
			upLine.SetActive(true);
		else if (dir == GridSquare.GridDirection.Right)
			rightLine.SetActive(true);
		else if (dir == GridSquare.GridDirection.Down)
			downLine.SetActive(true);
		else if (dir == GridSquare.GridDirection.Left)
			leftLine.SetActive(true);
	}


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
	}


	private void SetTexture(Texture t) {
		component.GetComponent<Renderer>().material.SetTexture("_MainTex", t);
	}
}
