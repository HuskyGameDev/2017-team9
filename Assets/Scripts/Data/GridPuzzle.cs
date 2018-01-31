using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzle : MonoBehaviour {

	public float squareScale = 1.0f;
	public int width = 10;
	public int height = 10;


	/// <summary>
	/// Generates a grid with the class values
	/// </summary>
	public void GenerateGrid() {
		GridSquare[] bottomRow = new GridSquare[width];
		GridSquare last = null;

		for (int y = 0; y < height; y++) {
			GridSquare[] currentRow = new GridSquare[width];
			for (int x = 0; x < width; x++) {
				GridSquare newSquare = getSquare();
				//Make our left connection, and the previous one's right connection
				if (x-1 >= 0) {
					currentRow[x - 1].neighbors[(int)GridSquare.GridDirection.Right] = newSquare;
					newSquare.neighbors[(int)GridSquare.GridDirection.Left] = currentRow[x - 1];
				}
				if (bottomRow[x] != null) {
					bottomRow[x].neighbors[(int)GridSquare.GridDirection.Up] = newSquare;
					newSquare.neighbors[(int)GridSquare.GridDirection.Down] = bottomRow[x];
				}
				newSquare.gameObject.transform.parent = this.transform;
				newSquare.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
				newSquare.transform.localPosition = new Vector3(x * squareScale, y * squareScale, 0.0f);
				newSquare.transform.name = "(" +x+ "," +y+ ")GridSquare";
				currentRow[x] = newSquare;
			}
			bottomRow = currentRow;
		}
	}

	public void DestroyGrid() {
		foreach (GridSquare gO in this.transform.GetComponentsInChildren<GridSquare>())
			DestroyImmediate(gO.gameObject);
	}


	private GridSquare getSquare() {
		GridSquare newSquare = null;
		GameObject gO = Instantiate(Resources.Load("GridSquare", typeof(GameObject))) as GameObject;
		newSquare = gO.GetComponent<GridSquare>();
		return newSquare;
	}
}
