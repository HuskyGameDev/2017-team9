using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;


public class GridLine {

	public DataComponent A;
	public DataComponent B;

	public Color color;

	public bool DeletionFlag = false;

	public LinkedList<GridSquare> squares = new LinkedList<GridSquare>();



	/// <summary>
	/// Adds a component to one of the ends of this line
	/// </summary>
	/// <param name="dc"></param>
	public void AddDataComponent(DataComponent dc) {
		Debug.Log("AddingDataComponent!");
		//Don't add the same one more than once
		if (A == dc || B == dc)
			return;

		if (A == null)
			A = dc;
		else
			B = dc;
	}

	/// <summary>
	/// Validates that the two data components are connected via the squares list
	/// </summary>
	/// <returns></returns>
	public bool ValidatePathBetweenDataComponents() {
		//[TODO] THIS IS NOT CORRECT. IT DOES NOT CHECK FOR BREAKS IN THE LINE

		if (A == null || B == null) {
			Debug.Log("Validate failed via not having two components");
			return false;
		}

		//Now we need to make sure that both sockets are different.
		//We also can check to make sure the line exists since we have to call FindLineDirection anyway.
		GridSquare.GridDirection dirA, dirB;
		if (A.attachedSquare.FindLineDirection(this, out dirA) == false || B.attachedSquare.FindLineDirection(this, out dirB) == false || A.attachedSquare.socketState[(int)dirA] == B.attachedSquare.socketState[(int)dirB]) {
			//If they have the same state (Input->Input/Output->Output) we fail them
			return false;
		}


		//First we need to make sure which other we need to find
		DataComponent other = null;
		if (squares.First.Value.dataComponent == A) {
			other = B;
		}
		else if (squares.First.Value.dataComponent == B) {
			other = A;
		}
		else {
			Debug.Log("Validate failed via Failed to have socket in First Square");
			//Neither are in the first square, our line is invalid!
			return false;
		}

		Debug.Log((squares.Last.Value.dataComponent == other) ? "Validate Pased!" : "Validate failed via Not Finding other at the end.");
		Debug.Log(squares.Last.Value.dataComponent);
		//If the last one's value is the other, we are a valid connection
		return squares.Last.Value.dataComponent == other;
	}

	/// <summary>
	/// Removes the Datacomponent from the pair
	/// </summary>
	public void RemoveDataComponent(DataComponent dc) {
		if (dc == A) {
			A = null;
		}
		if (dc == B) {
			B = null;
		}
	}

	/// <summary>
	/// Adds a square to this line, this method is not responsible for setting connections. It also does not trim.
	/// </summary>
	/// <param name="square"></param>
	public void AddSquare(GridSquare square) {
		squares.AddLast(square);
	}


	/// <summary>
	/// Removes the squares that come after a certain point in the squares list. Inclusive.
	/// </summary>
	/// <param name="square"></param>
	public void Trim(GridSquare square) {
		Debug.LogError("Trim Executed!");

		//We trim everything that comes after the sent square (including it)
		LinkedListNode <GridSquare> current = squares.First;
		LinkedList<GridSquare> newList = new LinkedList<GridSquare>();
		while (current != null) {
			if (current.Value == square) {
				//We found the square!
				//set the newList equal to our squares list. Signal every square starting here that it no longer has a line.
				while (current != null) {
					Debug.Log("Hit trim start & " + current.Value.gameObject.transform.name);
					//Remove line takes care of removing the datacomponent if it has one
					current.Value.RemoveLine(this);
					current = current.Next;
				}
				//We then override our list;
				squares = newList;
				return;
			}

			newList.AddLast(current.Value);
			current = current.Next;
		}
	}

	/// <summary>
	/// Returns the other assuming that the passed argument is one of them
	/// </summary>
	/// <param name="dc"></param>
	public DataComponent GetOther(DataComponent dc) {
		return (dc == A) ? B : A;
	}

	/// <summary>
	/// Removes this line from reference on the grid
	/// </summary>
	public void DeleteFromGrid() {
		DeletionFlag = true; //Let the terminal know that we have been deleted for some reason.
		Debug.LogError("DeletingLine");
		//Since trim handles the removal of squares, and is inclusive of the square past, if we pass the first square everything will be removed properly
		while (squares.First != null) {
			//Remove line takes care of removing the datacomponent if it has one
			squares.First.Value.RemoveLine(this);
			squares.RemoveFirst();
		}
		//If the line had a connecion, we need to notify the components they no longer have one.
		if (A != null && B != null) {
			A.ConnectionChange();
			B.ConnectionChange();
		}
	}
}
