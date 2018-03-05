using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataStructures;

public class GridPuzzle : MonoBehaviour {

	public bool editable = true;

	public int width = 10;
	public int height = 10;

	public GridSquare[,] squares;

	int usedLineCount = 0;
	public static Dictionary<int, Color> usedLines = new Dictionary<int, Color>();



	/// <summary>
	/// Generates a grid of grid squares. Automattically links them properly.
	/// </summary>
	public void GenerateGrid() {
		StaticGenerateGrid("", this, this.gameObject, width, height);
	}

	public void GenerateGrid(string namePrefix, int width, int height) {
		this.width = width;
		this.height = height;
		StaticGenerateGrid(namePrefix,this, this.gameObject, width, height);
	}

	public static void StaticGenerateGrid(string namePrefix, GridPuzzle puzzle, GameObject gridSquareHolder, int width, int height) {
		puzzle.squares = new GridSquare[width,height];
		//Calculate an offset. It is half of the count of width and height, but we need to account for odd numbers
		Vector3 offset = new Vector3( (width * -0.5f) + ((width % 2 == 0) ? 0 : 0.5f), (height *-0.5f) + ((height % 2 == 0) ? 0 : 0.5f), 0.0f);
		GridSquare[] lastRow = new GridSquare[width];

		for (int y = 0; y < height; y++) {
			GridSquare[] currentRow = new GridSquare[width];
			for (int x = 0; x < width; x++) {
				GridSquare newSquare = getSquare();
				//Make our left connection, and the previous one's right connection
				if (x - 1 >= 0) {
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
				newSquare.gameObject.transform.parent = gridSquareHolder.transform;
				//Set the position and scale
				newSquare.transform.localScale = new Vector3(1.0f, 1.0f, newSquare.transform.localScale.z);
				newSquare.transform.localPosition = offset + (new Vector3(x, y, 0.0f));

				//We blank out the rotation so the grid will look right if this game object is rotated oddly
				newSquare.transform.localRotation = Quaternion.Euler(Vector3.zero);

				newSquare.transform.name = namePrefix + "(" + x + "," + y + ")GridSquare";
				currentRow[x] = newSquare;
				newSquare.puzzle = puzzle;
				puzzle.squares[x, y] = newSquare;
			}
			lastRow = currentRow;
		}

		//Update all the visuals now that we are done generating
		foreach (GridSquare d in gridSquareHolder.GetComponentsInChildren<GridSquare>())
			d.RebuildSquare();
	}

	/// <summary>
	/// Returns an unused ID for a line on this puzzle.
	/// </summary>
	/// <returns></returns>
	public int GetLine() {
		Debug.Log("Creating new line");
		usedLines.Add(++usedLineCount, Random.ColorHSV());
		return usedLineCount;
	}




	/// <summary>
	/// Removes all of the grid square game objects
	/// </summary>
	public void DestroyGrid() {
		foreach (GridSquare s in this.gameObject.GetComponentsInChildren<GridSquare>())
			DestroyImmediate(s.gameObject);
	}


	/// <summary>
	/// Loads and creates a gridSquare prefab
	/// </summary>
	/// <returns></returns>
	private static GridSquare getSquare() {
		GridSquare newSquare = null;
		GameObject gO = Instantiate(Resources.Load("Grid/GridSquare", typeof(GameObject))) as GameObject;
		newSquare = gO.GetComponent<GridSquare>();
		return newSquare;
	}

	/// <summary>
	/// Calls rebuild square on all nodes in the grid
	/// </summary>
	public void RebuildGrid() {
		foreach (GridSquare s in this.gameObject.GetComponentsInChildren<GridSquare>())
			s.RebuildSquare();
	}


	//Returns the squares on the edge of a grid. Uses GridDirection since it is an enum that already exists that implies direction.
	public GridSquare[] GetEdge(GridSquare.GridDirection direction) {
		GridSquare[] ret;

		if (direction == GridSquare.GridDirection.Up) {
			ret = new GridSquare[width];
			int y = height - 1;
			for (int x = 0; x < width; x++) {
				ret[x] = squares[x, y];
			}
		}
		else if (direction == GridSquare.GridDirection.Down) {
			ret = new GridSquare[width];
			int y = 0;
			for (int x = 0; x < width; x++) {
				ret[x] = squares[x, y];
			}
		}
		else if (direction == GridSquare.GridDirection.Right) {
			ret = new GridSquare[height];
			int x = width - 1;
			for (int y = 0; y < height; y++) {
				ret[y] = squares[x, y];
			}
		}
		else { //direction == GridSquare.GridDirection.Left
			ret = new GridSquare[height];
			int x = 0;
			for (int y = 0; y < height; y++) {
				ret[y] = squares[x, y];
			}
		}

		return ret;
	}


}
