using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visuals for a gridSquare
/// </summary>
[RequireComponent(typeof(GridSquare))]
public class GridSquareVisuals : MonoBehaviour {

	public GameObject visualsHolder;

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

		GridSquare square = gameObject.GetComponent<GridSquare>();


		//Destroy the current visual grid
		foreach (Transform c in visualsHolder.transform) {
			GameObject.DestroyImmediate(c.gameObject);
		}

		if (square.type != GridSquare.GridType.Empty) {
			//Add the right visual gameobject
			loadVisualGameObject("Grid/Component", 0.25f);
		}
		else {
			loadVisualGameObject("Grid/Regular", 0.25f);
		}


		GridSquareRegular child = gameObject.GetComponentInChildren<GridSquareRegular>();


		child.upLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Up] != GridSquare.SocketState.None);
		child.downLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Down] != GridSquare.SocketState.None);
		child.leftLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Left] != GridSquare.SocketState.None);
		child.rightLine.SetActive(square.socketState[(int)GridSquare.GridDirection.Right] != GridSquare.SocketState.None);

		if (square.type != GridSquare.GridType.Empty) {
			((GridSquareComponent)child).component.GetComponent<Renderer>().material.SetTexture("_MainTex", getTexture());
			//Now we draw input/output arrows
			//[TODO] Make this actually look good

			Vector3[] arrowPlacementDirections = new Vector3[] { new Vector3(0.0f,1.3f,0.0f), new Vector3(1.3f, 0.0f, 0.0f), new Vector3(0.0f, -1.3f, 0.0f), new Vector3(-1.3f, 0.0f, 0.0f) };

			for (int i = 0; i < square.socketState.Length; i++) {
				if (square.socketState[i] == GridSquare.SocketState.Input || square.socketState[i] == GridSquare.SocketState.Output) {
					GameObject arrow = loadVisualGameObject("Grid/Arrow", 0.15f);
					//Set the proper position.
					arrow.transform.localPosition = arrowPlacementDirections[i] * 0.25f;
					//Calcualte the correct z rotation
					float zRotation = 270 - (i * 90);
					if (square.socketState[i] == GridSquare.SocketState.Input)
						zRotation += 180;
					arrow.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, zRotation);
				}
			}

			
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


	//Loads the proper game object that handles the visuals for this square
	private GameObject loadVisualGameObject(string path, float scaleMulitplier) {
		GameObject gO = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
		gO.transform.parent = visualsHolder.transform;
		gO.transform.localScale = new Vector3(1.0f * scaleMulitplier, 1.0f * scaleMulitplier, gO.transform.localScale.z);
		gO.transform.localPosition = Vector3.zero;

		//We blank out the rotation so the grid will look right if this game object is rotated oddly
		gO.transform.localRotation = Quaternion.Euler(Vector3.zero);
		return gO;
	}
}
