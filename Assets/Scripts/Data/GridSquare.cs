﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;

[RequireComponent(typeof(GridSquareVisuals))]
public class GridSquare : MonoBehaviour {

	/// <summary>
	/// Shorthand refence to directions for array use
	/// </summary>
	public enum GridDirection { Up, Right, Down, Left }
	/// <summary>
	/// Shorthand array to access the opposite of a direction
	/// </summary>
	public static readonly GridDirection[] oppositeDirection = new GridDirection[] { GridDirection.Down, GridDirection.Left, GridDirection.Up, GridDirection.Right };
	/// <summary>
	/// Shorthand array to access the cross dirctions of a direction
	/// </summary>
	public static readonly GridDirection[][] crossDirections = new GridDirection[][] { new GridDirection[] { GridDirection.Left, GridDirection.Right}, new GridDirection[] { GridDirection.Down, GridDirection.Up }, new GridDirection[] { GridDirection.Left, GridDirection.Right }, new GridDirection[] { GridDirection.Down, GridDirection.Up } };

	/// <summary>
	/// Enum of all states a grid square can be
	/// </summary>
	public enum GridType { Empty, Unusable, Adder, Combiner, Connector, Deleter, Linker, Shifter, Source}

	public DataComponent dataComponent;

	/// <summary>
	/// The GridType this square is
	/// </summary>
	public GridType type;

	/// <summary>
	/// The states a socket can be in
	/// </summary>
	public enum SocketState { None, Input, Output}

	/// <summary>
	/// The states for each socket direction
	/// </summary>
	public SocketState[] socketState = new SocketState[4];
	/// <summary>
	/// The adjacent squares this square has. It should be auto-generated.
	/// </summary>
	public GridSquare[] neighbors = new GridSquare[4];

	/// <summary>
	/// The GridLine that is on this square.
	/// </summary>
	public GridLine[] line = new GridLine[4];


	/// <summary>
	/// Creates a connection between two grid squares along the given direction with the specified line. A(->dir->)B
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="A"></param>
	/// <param name="B"></param>
	/// <param name="newLine"></param>
	/// <returns></returns>
	public static bool Connect(GridDirection direction, GridSquare A, GridSquare B, GridLine newLine) {

		//If we are trying to connect two non neighbors, we just abort!
		{
			GridDirection tdir;
			if (GridSquare.AreNeighbors(A, B, out tdir) == false) {
				Debug.LogError("Attempt to connect with non neightbors!");
				newLine.DeleteFromGrid();
				return false;
			}
		}

		if (B.type == GridType.Unusable) {
			Debug.Log("X->Unusable");
			//We can not use one of  these grid squares
			return false;
		}

		//First we need to see how many connections already exist on these squares. We take a count as well as track the line that exists on it.
		int countB = 0;
		GridLine foundB = null;
		for (int i = 0; i < A.line.Length; i++) {
			if (B.line[i] != null) {
				countB++;
				foundB = B.line[i];
			}
		}


		if (B.type == GridType.Empty) {
			Debug.Log("X->Empty");
			//This has three cases. We have looped back onto ourselves. We have collided with another line, or everything is fine

			if (countB > 0 && foundB == newLine) {
				//The case we have wrapped back on ourselves
				//So we need to trim the line.
				newLine.Trim(B);
				//Substitute A for what is left in the line.
				GridSquare sub = newLine.squares.Last.Value;
				//Figure out the new connection direction
				GridSquare.GridDirection newDirection;
				if (GridSquare.AreNeighbors(sub, B, out newDirection) == false) {
					//If they are not neigbors, something has gone wrong and we need to abort.
					newLine.DeleteFromGrid();
					return false;
				}
				//Make the connection.
				//We know know the new connection to be made, so lets do it.
				sub.line[(int)newDirection] = newLine;
				B.line[(int)GridSquare.oppositeDirection[(int)newDirection]] = newLine;
			}
			else {
				//If we have encountered another line, it needs to get removed.
				if (countB > 0) {
					//Remove it
					for (int i = 0; i < B.line.Length; i++) {
						if (B.line[i].DeletionFlag == false)
							B.line[i].DeleteFromGrid();
					}
				}

				//Now we can just make the connection like normal!
				A.line[(int)direction] = newLine;
				B.line[(int)GridSquare.oppositeDirection[(int)direction]] = newLine;
			}
		}
		else if (B.type != GridType.Empty) {
			Debug.Log("X->Socket");
			//So know we know the endpoint is a socket, so we just need to make sure there is one
			if (B.socketState[(int)GridSquare.oppositeDirection[(int)direction]] != GridSquare.SocketState.None) {
				//If there is, we can just make the connection!
				//Implicitly, for us to have made it this far into a connection with a socket, any line that could have existed in this spot has been trimmed.
				//so we can just make the connection!
				A.line[(int)direction] = newLine;
				B.line[(int)GridSquare.oppositeDirection[(int)direction]] = newLine;
			}
			else {
				return false;
			}
		}
		return true;
	}


	/// <summary>
	/// Checks if you can make a connection on this square on the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public bool CanConnect(GridDirection dir) {
		if (type == GridType.Unusable)
			return false;
		else if (type == GridType.Empty)
			return true;
		else
			return (socketState[(int)dir] != SocketState.None);
	}

	/// <summary>
	/// Adds a line onto this square in the specified direction. Does not care about overriding
	/// </summary>
	/// <param name="add"></param>
	/// <param name="dir"></param>
	public void AddLine(GridLine add, GridDirection dir) {
		//the unusuable type cannot have any connections, ever
		if (type == GridType.Unusable) {
			return;
		}
		//Add the line
		line[(int)dir] = add;
		this.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
	}

	/// <summary>
	/// Removes a line from our square.
	/// </summary>
	/// <param name="remove"></param>
	public void RemoveLine(GridLine remove) {
		Debug.Log("Removing Line!");
		for (int i = 0; i < line.Length; i++) {
			if (line[i] == remove) {
				line[i] = null;
			}
		}
		this.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
	}



	/// <summary>
	/// Static method that checks if two Squares are neighbors
	/// </summary>
	/// <returns></returns>
	public static bool AreNeighbors(GridSquare A, GridSquare B, out GridDirection dir) {

		//Base cases
		if (A == null || B == null || A == B) {
			dir = GridDirection.Up;
			return false;
		}

		//For each possible direction
		for (int i = 0; i < A.neighbors.Length; i++) {
			//If we actually have a square in that direction
			if (A.neighbors[i] == null || B.neighbors[(int)oppositeDirection[i]] == null)
				continue;
			//check if they are in fact next too eachother (checks both ways for safety, this will hopefully bring more noticibilty to grid generation issues)
			if (A.neighbors[i] == B && B.neighbors[(int)oppositeDirection[i]] == A) {
				dir = (GridDirection)i;
				return true;
			}
		}
		//Else we return false, and the default value for a direction.
		dir = GridDirection.Up;
		return false;
	}


	/// <summary>
	/// Changes the component to a new type. This should be called automatically with an editor script.
	/// </summary>
	/// <param name="newType"></param>
	public void ChangeComponent(GridType newType) {
		Debug.Log("Attempting to ChangeComponent");
		//Clear all old components (should only be one but this is more safe)
		foreach (DataComponent dc in this.gameObject.GetComponents<DataComponent>()) {
			StartCoroutine(DestroyEndOfFrame(dc));
		}

		//Add the new type of component
		if (newType == GridType.Adder) {
			dataComponent = this.gameObject.AddComponent<DataAdder>();
		}
		else if (newType == GridType.Combiner) {
			dataComponent = this.gameObject.AddComponent<DataCombiner>();
		}
		else if (newType == GridType.Connector) {
			dataComponent = this.gameObject.AddComponent<DataConnector>();
		}
		else if (newType == GridType.Deleter) {
			dataComponent = this.gameObject.AddComponent<DataDeleter>();
		}
		else if (newType == GridType.Linker) {
			dataComponent = this.gameObject.AddComponent<DataLinker>();
		}
		else if (newType == GridType.Shifter) {
			dataComponent = this.gameObject.AddComponent<DataShifter>();
		}
		else if (newType == GridType.Source) {
			dataComponent = this.gameObject.AddComponent<DataSource>();
		}
		else if (newType == GridType.Unusable) {
			dataComponent = this.gameObject.AddComponent<DataUnusable>();
		}

		//Make sure the new component knows about us!
		if (dataComponent != null)
			dataComponent.attachedSquare = this;
	}

	/// <summary>
	/// Validates that component is the same type as the enum. True if they are the same.
	/// </summary>
	public bool ValidateTypeToComponentIntegrity() {
		bool consistent = false;

		if (type == GridType.Empty && dataComponent == null) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (dataComponent == null) {
			//From here, if we do not have a component attached we implicitly know we need to make a chanmge
			consistent = false;
		}
		else if (type == GridType.Adder && dataComponent.GetType() == typeof(DataAdder)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Combiner && dataComponent.GetType() == typeof(DataCombiner)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Connector && dataComponent.GetType() == typeof(DataConnector)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Deleter && dataComponent.GetType() == typeof(DataDeleter)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Linker && dataComponent.GetType() == typeof(DataLinker)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Shifter && dataComponent.GetType() == typeof(DataShifter)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Source && dataComponent.GetType() == typeof(DataSource)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Unusable && dataComponent.GetType() == typeof(DataUnusable)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}

		//Debug.Log("Consistent: " + (consistent));
		return consistent;
	}

	/// <summary>
	/// Checks if the passed line is on this square.
	/// </summary>
	/// <param name="checkLine"></param>
	/// <returns></returns>
	public bool FindLineDirection(GridLine checkLine, out GridDirection dir) {
		for (int i = 0; i < line.Length; i++) {
			if (line[i] == checkLine) {
				dir = (GridDirection)i;
				return true;
			}
		}
		dir = GridDirection.Up;
		return false;
	}


	/// <summary>
	/// Method to update the state of the GridSquare based on the classes data
	/// </summary>
	public void ValidateSquare() {
		//If our Type is different from our component, fix that
		if (ValidateTypeToComponentIntegrity() == false)
			ChangeComponent(type);

		//Update the visuals when we make a change
		this.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
	}



	IEnumerator DestroyEndOfFrame(MonoBehaviour go) {
		yield return new WaitForEndOfFrame();
		DestroyImmediate(go);
	}
}
