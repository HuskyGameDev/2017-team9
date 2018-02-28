using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzle : MonoBehaviour {

	public bool editable = true;

	public float squareScale = 1.0f;
	public int width = 10;
	public int height = 10;





	/// <summary>
	/// Generates a grid of grid squares. Automattically links them properly.
	/// </summary>
	public void GenerateGrid() {
		GridSquare[] lastRow = new GridSquare[width];

		for (int y = 0; y < height; y++) {
			GridSquare[] currentRow = new GridSquare[width];
			for (int x = 0; x < width; x++) {
				GridSquare newSquare = getSquare();
				//Make our left connection, and the previous one's right connection
				if (x-1 >= 0) {
					currentRow[x - 1].neighbors[(int)GridSquare.GridDirection.Right] = newSquare;
					newSquare.neighbors[(int)GridSquare.GridDirection.Left] = currentRow[x - 1];

					currentRow[x - 1].socketState[(int)GridSquare.GridDirection.Right] = GridSquare.SocketState.Line;
					newSquare.socketState[(int)GridSquare.GridDirection.Left] = GridSquare.SocketState.Line;
				}
				if (lastRow[x] != null) {
					//If the last row is not null, that means we can set some up/down connections
					lastRow[x].neighbors[(int)GridSquare.GridDirection.Up] = newSquare;
					newSquare.neighbors[(int)GridSquare.GridDirection.Down] = lastRow[x];

					lastRow[x].socketState[(int)GridSquare.GridDirection.Up] = GridSquare.SocketState.Line;
					newSquare.socketState[(int)GridSquare.GridDirection.Down] = GridSquare.SocketState.Line;
				}
				//Put this grid sqaure under this game obnject
				newSquare.gameObject.transform.parent = this.transform;
				//Set the position and scale
				newSquare.transform.localScale = new Vector3(squareScale, squareScale, newSquare.transform.localScale.z);
				newSquare.transform.localPosition = new Vector3(x * squareScale, y * squareScale, 0.0f);

				//We blank out the rotation so the grid will look right if this game object is rotated oddly
				newSquare.transform.localRotation = Quaternion.Euler(Vector3.zero);

				newSquare.transform.name = "(" +x+ "," +y+ ")GridSquare";
				currentRow[x] = newSquare;
			}
			lastRow = currentRow;
		}

		//Update all the visuals now that we are done generating
		foreach (GridSquare d in this.gameObject.GetComponentsInChildren<GridSquare>())
			d.ValidateSquare();
	}

	/// <summary>
	/// Removes all of the grid square game objects
	/// </summary>
	public void DestroyGrid() {
		foreach (GridSquare gO in this.transform.GetComponentsInChildren<GridSquare>())
			DestroyImmediate(gO.gameObject);
	}


	/// <summary>
	/// Loads and creates a gridSquare prefab
	/// </summary>
	/// <returns></returns>
	private GridSquare getSquare() {
		GridSquare newSquare = null;
		GameObject gO = Instantiate(Resources.Load("Grid/GridSquare", typeof(GameObject))) as GameObject;
		newSquare = gO.GetComponent<GridSquare>();
		return newSquare;
	}
}
