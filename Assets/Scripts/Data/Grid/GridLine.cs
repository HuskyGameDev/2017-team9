using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;


public class GridLine {
	public Color color = Color.blue;

	public bool DeletionFlag = false;

	public LinkedList<GridSquare> squares = new LinkedList<GridSquare>();

	/// <summary>
	/// Scans down the squares list looking for a socket from a starting point
	/// </summary>
	/// <returns></returns>
	public void CheckForOpposingSocket(GridSquare square, GridSquare.GridDirection directon, out GridSquare retSquare, out GridSquare.GridDirection retDirection) {

		//First, grab this squares node
		LinkedListNode<GridSquare> node = squares.Find(square);
		//If this square isnt on the list, abandon search
		if (node == null) {
			Debug.Log("CFOS: Failed by not being on the list");
			retSquare = null;
			retDirection = GridSquare.GridDirection.Up;
			return;
		}

		//So now we need to crawl along the correct direction of our list.
		//So we need to figure out what direction that is.
		bool isForward;

		if (node.Next != null && node.Next.Value.line[(int)GridSquare.oppositeDirection[(int)directon]] == this) {
			isForward = true;
		}
		else if (node.Previous != null && node.Previous.Value.line[(int)GridSquare.oppositeDirection[(int)directon]] == this) {
			isForward = false;
		}
		else {
			Debug.Log("CFOS: Failed by not have a direction that made sense.");
			retSquare = null;
			retDirection = GridSquare.GridDirection.Up;
			return;
		}

		//So now we can start the search down the line
		LinkedListNode<GridSquare> lastNode = node;
		LinkedListNode<GridSquare> currentNode = (isForward) ? node.Next : node.Previous;
		while (currentNode != null) {
			//So now we check the direction between these two nodes, we do this by calling the ArNeighbors method
			GridSquare.GridDirection outDirection;
			if (GridSquare.AreNeighbors(currentNode.Value, lastNode.Value, out outDirection) == false) {
				//Extra fail condition in that we have found two nodes that are not neighbors
				//If this is true, we have an incorrect line...
				DeletionFlag = true;
				DeleteFromGrid();
				retSquare = null;
				retDirection = GridSquare.GridDirection.Up;
				Debug.Log("CFOS: Failed by non neighbors in the list");
				return;
			}
			
			//So we check if this node has a socket on the direction we calculated
			if (currentNode.Value.socketState[(int)outDirection] == GridSquare.SocketState.Input || currentNode.Value.socketState[(int)outDirection] == GridSquare.SocketState.Output) {
				//It does, so we return this new socket
				retDirection = outDirection;
				retSquare = currentNode.Value;
				return;
			}

			//Go to the next part of the loop
			lastNode = currentNode;
			currentNode = (isForward) ? currentNode.Next : currentNode.Previous;
		}

		//We did not find a socket during the search so we have failed
		retSquare = null;
		retDirection = GridSquare.GridDirection.Up;
		Debug.Log("CFOS: Failed by not finding a socket.");
		return;
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
	/// Removes this line from reference on the grid
	/// </summary>
	public void DeleteFromGrid() {
		DeletionFlag = true; //Let the terminal know that we have been deleted for some reason.
		Debug.LogError("DeletingLine");
		//Since trim handles the removal of squares, and is inclusive of the square past, if we pass the first square everything will be removed properly
		while (squares.First != null) {
			//Remove line takes care of removing the datacomponent if it has one
			squares.First.Value.RemoveLine(this);
			if (squares.First.Value.dataComponent != null) {
				squares.First.Value.dataComponent.ConnectionChange();
			}
			squares.RemoveFirst();
		}
	}
}
