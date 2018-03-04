using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;


public class GridLine {
	public Color color = Color.blue;

	public bool DeletionFlag = false;

	public GridPuzzle owner;


	public LinkedList<LineSegment> lineSegments = new LinkedList<LineSegment>();


	/// <summary>
	/// Searches the line segment listt for the correct square
	/// </summary>
	/// <param name="square"></param>
	/// <returns></returns>
	public LinkedListNode<LineSegment> findNodeFromSquare(GridSquare square) {
		LinkedListNode<LineSegment> ret = lineSegments.First;
		while (ret != null) {
			if (ret.Value.square == square)
				return ret;
			ret = ret.Next;
		}
		return null;
	}

	public GridSquare getLast() {
		return lineSegments.Last.Value.square;
	}

	/*/// <summary>
	/// Scans down the squares list looking for a socket from a starting point
	/// </summary>
	/// <returns></returns>
	public void CheckForOpposingSocket(GridSquare square, GridSquare.GridDirection directon, out GridSquare retSquare, out GridSquare.GridDirection retDirection) {

		//First, grab this squares node
		LinkedListNode<LineSegment> node = findNodeFromSquare(square);
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

		if (node.Next != null && node.Next.Value.square.line[(int)GridSquare.oppositeDirection[(int)directon]] == this) {
			isForward = true;
		}
		else if (node.Previous != null && node.Previous.Value.square.line[(int)GridSquare.oppositeDirection[(int)directon]] == this) {
			isForward = false;
		}
		else {
			Debug.Log("CFOS: Failed by not have a direction that made sense.");
			retSquare = null;
			retDirection = GridSquare.GridDirection.Up;
			return;
		}

		//So now we can start the search down the line
		LinkedListNode<LineSegment> lastNode = node;
		LinkedListNode<LineSegment> currentNode = (isForward) ? node.Next : node.Previous;
		while (currentNode != null) {
			//So now we check the direction between these two nodes, we do this by calling the ArNeighbors method
			GridSquare.GridDirection outDirection;
			if (GridSquare.AreNeighbors(currentNode.Value.square, lastNode.Value.square, out outDirection) == false) {
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
			if (currentNode.Value.square.socketState[(int)outDirection] == GridSquare.SocketState.Input || currentNode.Value.square.socketState[(int)outDirection] == GridSquare.SocketState.Output) {
				//It does, so we return this new socket
				retDirection = outDirection;
				retSquare = currentNode.Value.square;
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
	}*/

	
	/// <summary>
	/// Checks for the next component on a line based on a starting square and a direction.
	/// </summary>
	/// <param name="square"> The square to check for the oposing socket on. </param>
	/// <param name="directon"> The direction we should attempt to travel. </param>
	/// <param name="retDirection"> the direction on which this line falls on the socket we found. </param>
	/// <returns></returns>
	public GridSquare CheckForOtherComponent(GridSquare square, GridSquare.GridDirection directon, out GridSquare.GridDirection retDirection) {
		//First, grab this squares node
		LinkedListNode<LineSegment> node = findNodeFromSquare(square);
		//If this square isnt on the list, abandon search
		if (node == null) {
			Debug.Log("CFOC: Failed by not being on the list");
			retDirection = GridSquare.GridDirection.Down;
			return null;
		}

		//So now we need to crawl along the correct direction of our list.
		//So we need to figure out what direction that is.
		bool isForward;
		if (node.Next != null && node.Next.Value.square.line[(int)GridSquare.oppositeDirection[(int)directon]] == this) {
			isForward = true;
		}
		else if (node.Previous != null && node.Previous.Value.square.line[(int)GridSquare.oppositeDirection[(int)directon]] == this) {
			isForward = false;
		}
		else {
			Debug.Log("CFOC: Failed by not have a direction that made sense.");
			retDirection = GridSquare.GridDirection.Down;
			return null;
		}


		LinkedListNode<LineSegment> lastNode = node;
		LinkedListNode<LineSegment> currentNode = (isForward) ? node.Next : node.Previous;
		while (currentNode != null) {
			if (currentNode.Value.square.type != GridSquare.GridType.Empty) {
				GridSquare.AreNeighbors(currentNode.Value.square, lastNode.Value.square, out retDirection);
				return currentNode.Value.square;
			}
			lastNode = currentNode;
			currentNode = (isForward) ? currentNode.Next : currentNode.Previous;
		}

		retDirection = GridSquare.GridDirection.Down;
		return null;
	}



	/// <summary>
	/// Adds a square to this line, this method is not responsible for setting connections. It also does not trim.
	/// </summary>
	/// <param name="square"></param>
	public void AddSquare(GridSquare square) {
		LineSegment segment = new LineSegment();
		segment.square = square;

		GameObject centerSection = loadLineSection();
		centerSection.transform.position = segment.square.transform.position;
		segment.onSectionVisual = centerSection;

		if (lineSegments.Last != null) {
			GameObject lineSection = loadLineSection();
			lineSection.transform.position = Vector3.Lerp(segment.square.transform.position, lineSegments.Last.Value.square.transform.position, 0.5f);


			Vector3 scale = new Vector3(0.0f, 0.0f, lineSection.transform.localScale.z);
			GridSquare.GridDirection dir;
			GridSquare.AreNeighbors(lineSegments.Last.Value.square, square, out dir);

			if (dir == GridSquare.GridDirection.Up || dir == GridSquare.GridDirection.Down) {
				scale.x = 0.175f;
				scale.y = 1.0f;
			}
			else if (dir == GridSquare.GridDirection.Left || dir == GridSquare.GridDirection.Right) {
				scale.x = 1.0f;
				scale.y = 0.175f; 
			}

			lineSection.transform.localScale = scale;

			segment.priorLineVisual = lineSection;
		}

		lineSegments.AddLast(segment);

	}


	/// <summary>
	/// Removes the squares that come after a certain point in the squares list. Inclusive.
	/// </summary>
	/// <param name="square"></param>
	public void TrimInclusive(GridSquare square) {
		Debug.LogError("Trim Executed!");

		//We trim everything that comes after the sent square (including it)
		LinkedListNode <LineSegment> current = lineSegments.First;
		LinkedList<LineSegment> newList = new LinkedList<LineSegment>();
		while (current != null) {
			if (current.Value.square == square) {
				//We found the square!
				//set the newList equal to our squares list. Signal every square starting here that it no longer has a line.
				while (current != null) {
					Debug.Log("Hit trim start & " + current.Value.square.gameObject.transform.name);
					//Remove line takes care of removing the datacomponent if it has one
					ClearLineSegment(current.Value);
					current.Value.square.RemoveLine(this);
					current = current.Next;
				}
				//We then override our list;
				lineSegments = newList;
				return;
			}

			newList.AddLast(current.Value);
			current = current.Next;
		}
	}

	/// <summary>
	/// Removes the squares that come after a certain point in the squares list. Exclusive
	/// </summary>
	/// <param name="square"></param>
	public void TrimExclusive(GridSquare square) {
		LinkedListNode<LineSegment> c = lineSegments.First;
		while (c != null) {
			if (c.Value.square == square) {
				//We have found us, so now we call trim inclusive on the one after this
				if (c.Next != null)
					TrimInclusive(c.Next.Value.square);
			}
			c = c.Next;
		}
	}

	private void ClearLineSegment(LineSegment segment) {
		GameObject.Destroy(segment.onSectionVisual);
		GameObject.Destroy(segment.priorLineVisual);
		segment.square = null;
	}


	/// <summary>
	/// Removes this line from reference on the grid
	/// </summary>
	public void DeleteFromGrid() {
		DeletionFlag = true; //Let the terminal know that we have been deleted for some reason.
		Debug.LogError("DeletingLine");
		//Since trim handles the removal of squares, and is inclusive of the square past, if we pass the first square everything will be removed properly
		while (lineSegments.First != null) {
			//Remove line takes care of removing the datacomponent if it has one
			lineSegments.First.Value.square.RemoveLine(this);
			if (lineSegments.First.Value.square.dataComponent != null) {
				lineSegments.First.Value.square.dataComponent.ConnectionChange();
			}
			ClearLineSegment(lineSegments.First.Value);
			lineSegments.RemoveFirst();
		}

		//Remove ourselves from the puzzle line registry
		owner.RemoveLine(this);
	}

	public struct LineSegment {
		//the grid square on the line
		public GridSquare square;
		//the visual for perfectly on this section
		public GameObject onSectionVisual;
		//visual line section for between this square and the one before
		public GameObject priorLineVisual;
	}


	/// <summary>
	/// Loads a line section from the resources
	/// </summary>
	/// <returns></returns>
	private GameObject loadLineSection() {
		GameObject gO = MonoBehaviour.Instantiate(Resources.Load("Grid/Line", typeof(GameObject))) as GameObject;
		gO.transform.parent = owner.lineVisualsHolder.transform;
		//gO.transform.localScale = new Vector3(1.0f * scaleMulitplier, 1.0f * scaleMulitplier, gO.transform.localScale.z);
		gO.transform.localPosition = Vector3.zero;

		//We blank out the rotation so the grid will look right if this game object is rotated oddly
		gO.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gO.transform.localScale = new Vector3(0.175f, 0.175f, 0.0005f);
		return gO;
	}
}
