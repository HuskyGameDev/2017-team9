﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;


public class GridLine {

	public DataComponent A;
	public DataComponent B;

	public Color color;

	LinkedList<GridSquare> squares = new LinkedList<GridSquare>();



	/// <summary>
	/// Adds a component to one of the ends of this line
	/// </summary>
	/// <param name="dc"></param>
	public void AddDataComponent(DataComponent dc) {
		if (A == null)
			A = dc;
		else
			B = dc;

		if (A != null && B != null) {
			A.ConnectionChange();
			B.ConnectionChange();
		}
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

		//We trim everything that comes after the sent square (including it)
		LinkedListNode <GridSquare> current = squares.First;
		LinkedList<GridSquare> newList = new LinkedList<GridSquare>();
		while (current != null) {
			if (current.Value == square) {
				//We found the square!
				//set the newList equal to our squares list. Signal every square starting here that it no longer has a line.
				while (current != null) {
					//Remove line takes care of removing the datacomponent if it has one
					current.Value.RemoveLine(this);
					current = current.Next;
				}
				//We then override our list;
				squares = newList;
				return;
			}
			newList.AddLast(current);
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
		Debug.Log("DeletingLine");
		//Since trim handles the removal of squares, and is inclusive of the square past, if we pass the first square everything will be removed properly
		Trim(squares.First.Value);
	}
}
